using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Interfaces;
using RentalMotor.Api.Services.Network;
using RentalMotor.Api.Services.Responsabilities;
using System.Text.RegularExpressions;

namespace RentalMotor.Api.Services.Implements
{
    public class RentalUserMotorService : IRentalUserMotorService
    {
        public readonly IUserMotorRepository _userMotorRepository;
        public readonly IContractPlanService _foorPlanService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMotorService _motorService;
        private readonly string UserId;
        private readonly string UserName;
        private readonly IAmazonS3 _s3Client;

        public RentalUserMotorService(IUserMotorRepository userMotorRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IContractPlanService foorPlanService, IMotorService motorService, IAmazonS3 s3Client)
        {
            _foorPlanService = foorPlanService;
            _userMotorRepository = userMotorRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _motorService = motorService;
            UserId = _httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault()!.Value;
            UserName = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Contains("emailaddress")).FirstOrDefault()!.Value;
            _s3Client = s3Client;
        }

        public async Task<ResponseContractUserMotorModel> AddUser(RequestUserMotorModel? userMotorModel)
        {
            try
            {
                var userMotor = _mapper.Map<User>(userMotorModel);
                userMotor.UserName = UserName;
                userMotor.UserId = UserId;

                var filePath = Path.Combine(Environment.CurrentDirectory, userMotorModel!.Cnh!.ImagenCnh.FileName);

                using Stream stream = new FileStream(filePath, FileMode.Create);
                userMotorModel.Cnh.ImagenCnh.CopyTo(stream);

                userMotor.Cnh!.ImagePath = filePath;

                var request = new PutObjectRequest()
                {
                    BucketName = "images-cnh-user",
                    Key = $"{UserId?.TrimEnd('/')}/{userMotorModel!.Cnh!.ImagenCnh.FileName}",
                    InputStream = userMotorModel!.Cnh!.ImagenCnh.OpenReadStream()
                };
                request.Metadata.Add("Content-Type", userMotorModel!.Cnh!.ImagenCnh.ContentType);
                await _s3Client.PutObjectAsync(request);

                var user = await Task.Run(() => _userMotorRepository.Add(userMotor));

                var result = _mapper.Map<ResponseContractUserMotorModel>(user);

                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResponseContractUserFoorPlanModel> AddContract(MotorModel? motorModel, RequestContractPlanUserMotorModel? contractPlanUserMotorModel)
        {
            var contractUserMotor = new ContractPlanUserMotor();
            var motorContractModel = new MotorContractModel();

            try
            {
                motorContractModel = _mapper.Map<MotorContractModel>(motorModel);

                var flag = _motorService.UpdateMotorFlag(motorContractModel).Result;

                if (flag)
                {
                    var countDay = contractUserMotor.FloorPlanCountDay;
                    var foorPlan = _foorPlanService.GetByCountDay(countDay);
                    var forCastEndDate = DateTime.Parse(contractUserMotor.ForecastEndDate);

                    var user = await Task.Run(() => BuildContracts.BuildContract(contractPlanUserMotorModel!, foorPlan, _mapper));
                    user.Id = UserId;

                    var userUpdated = await Task.Run(() => _userMotorRepository.Update(user));

                    var result = _mapper.Map<ResponseContractUserFoorPlanModel>(userUpdated.ContractUserFoorPlan);
                    return result;
                }
                return new ();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool Delete(string id)
        {
            return _userMotorRepository.Delete(id);
        }

        public async Task<IEnumerable<ResponseContractUserMotorModel>> Get(string? id = null, string? cpfCnpj = null, string? plate = null)
        {
            var userMotorModels = new List<ResponseContractUserMotorModel>();
            var usersMotors = await Task.Run(() => _userMotorRepository.Get(id, cpfCnpj, plate));
            if (usersMotors.Any())
            {
                foreach (var userMotor in usersMotors)
                {
                    var userMotorModel = _mapper.Map<ResponseContractUserMotorModel>(userMotor);
                    if (userMotorModel.ContractUserFoorPlanModel != null)
                    {
                        userMotorModel.ContractUserFoorPlanModel = new()
                        {
                            EndDate = userMotor.ContractUserFoorPlan!.EndDate,
                            FloorPlanCountDay = userMotor.ContractUserFoorPlan.FloorPlanCountDay,
                            ForecastEndDate = userMotor.ContractUserFoorPlan.ForecastEndDate,
                            Id = userMotor.ContractUserFoorPlan.Id,
                            MotorPlate = userMotor.ContractUserFoorPlan.MotorPlate,
                            PenaltyMissingDaysValue = userMotor.ContractUserFoorPlan.PenaltyMissingDaysValue,
                            PenaltyOverDaysValue = userMotor.ContractUserFoorPlan.PenaltyOverDaysValue,
                            StarDate = userMotor.ContractUserFoorPlan!.StarDate,
                            CountCurrentDays = userMotor.ContractUserFoorPlan!.CountCurrentDays
                        };
                        userMotorModels.Add(userMotorModel);
                    }
                    else
                        userMotorModels.Add(userMotorModel);

                }
                return userMotorModels;
            }
            return userMotorModels;
        }

        public async Task<S3ObjectModel> GetToDownloadImageCnh()
        {
            var user = await Task.Run(() => _userMotorRepository.Get(userId:UserId));
            if (user.Any())
            {

                var request = new ListObjectsV2Request()
                {
                    BucketName = "images-cnh-user",
                    Prefix = $"{UserId?.TrimEnd('/')}"
                };
                var result = await _s3Client.ListObjectsV2Async(request);
                
                var s3Objects = result.S3Objects.Select(s =>
                {
                    var urlRequest = new GetPreSignedUrlRequest()
                    {
                        BucketName = "images-cnh-user",
                        Key = s.Key,
                        Expires = DateTime.UtcNow.AddMinutes(1)
                    };
                    return new S3ObjectModel()
                    {
                        Name = s.Key.ToString(),
                        PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
                    };
                });

                return s3Objects.FirstOrDefault()!;
            }
            return new ();
        }

        public async Task<ResponseCnhModel> UpdateCnh(IFormFile cnhImage)
        {
            var userId = UserId;
            var result = await Task.Run(() => _userMotorRepository.Get(userId, null));

            var filePath = Path.Combine(Environment.CurrentDirectory, cnhImage.FileName);

            using Stream stream = new FileStream(filePath, FileMode.Create);
            cnhImage.CopyTo(stream);

            var userMotor = result.FirstOrDefault();

            userMotor!.Cnh!.ImagePath = filePath;

            userMotor = _userMotorRepository.Update(userMotor);

            var contractUserMotorModel = _mapper.Map<ResponseCnhModel>(userMotor!.Cnh);
            return contractUserMotorModel;
        }

        public async Task<ModelControllerValidation> ValidInputsController(RequestUserMotorModel? userMotorModel = null)
        {
            var modelValided = new ModelControllerValidation
            {
                Message = string.Empty,
                IsValid = false
            };

            var existUser = await Task.Run(() => _userMotorRepository.Get(userId:UserId));
            if (existUser.Any())
            {
                modelValided.Message = $"User {existUser.FirstOrDefault()!.UserName} is already registered";
                return modelValided;
            }

            var existCpfCnpj = await Task.Run(() => _userMotorRepository.Get(cpfCnpj:userMotorModel!.CpfCnpj));
            if (existCpfCnpj.Any())
            {
                modelValided.Message = $"Cpf or Cnpj {existCpfCnpj.FirstOrDefault()!.CpfCnpj} is already registered";
                return modelValided;
            }

            var isValidateDate = DateTime.TryParse(userMotorModel!.BirthDate, out DateTime birthDate);
            if (!isValidateDate)
            {
                modelValided.Message = "BirthDate invalid";
                return modelValided;
            }

            var existCnh = await Task.Run(() => _userMotorRepository.GetCnh(userMotorModel.Cnh!.NumberCnh));
            if (existCnh != null)
            {
                modelValided.Message = $"Cnh {existCnh!.NumberCnh} is already registered";
                return modelValided;
            }

            if (!userMotorModel.Cnh!.CnhCategories.Any(x => x.ToUpper().Equals("A")))
            {
                modelValided.Message = "You are not qualified to rent a motorcycle";
                return modelValided;
            }
            
            if (userMotorModel.Cnh!.CnhCategories.Any(x => !x.ToUpper().Equals("A") && !x.ToUpper().Equals("B")))
            {
                modelValided.Message = "Cnh category invalid";
                return modelValided;
            }

            modelValided.IsValid = true;
            return modelValided;

        }
      
        public async Task<ModelControllerValidation> ValidInputsController(RequestContractPlanUserMotorModel contractPlanUserMotorModel)
        {
            var modelValided = new ModelControllerValidation
            {
                Message = string.Empty,
                IsValid = false
            };

            var existUser = await Task.Run(() => _userMotorRepository.Get(userId:UserId));
            if (existUser.Any())
            {
                if (existUser.FirstOrDefault()!.ContractUserFoorPlan != null)
                {
                    modelValided.Message = $"User {existUser.FirstOrDefault()!.UserName} is already registered with contract registerd";
                    return modelValided;
                }


                var forecastEndValid = DateTime.TryParse(contractPlanUserMotorModel.ForecastEndDate, out DateTime forecastEndDate);
                if (!forecastEndValid)
                {
                    modelValided.Message = "ForecastEndDate invalid";
                    return modelValided;
                }

                if (forecastEndDate < DateTime.Now.AddDays(1))
                {
                    modelValided.Message = "Expected delivery date should be as of tomorrow.";
                    return modelValided;
                }

                var foorPlan = await Task.Run(() => _foorPlanService.GetByCountDay(contractPlanUserMotorModel.FloorPlanCountDay));
                if (foorPlan == null)
                {
                    modelValided.Message = "There are no plans for that number of days";
                    return modelValided;
                }

                var motorsAvailable = await Task.Run(() => _motorService.GetMotorsAvailableToRental());

                var motorPlateRequest = contractPlanUserMotorModel.MotorPlate;

                var motorAvailable = motorsAvailable.Where(x => x.Plate!.Equals(motorPlateRequest, StringComparison.CurrentCultureIgnoreCase));

                if (!motorAvailable.Any())
                {
                    modelValided.Message = $"Motorcycle {string.Join(',', motorPlateRequest)} is not available to rented";
                    return modelValided;
                }
                modelValided.MotorAvailable = motorAvailable.FirstOrDefault()!;
                modelValided.IsValid = true;
                return modelValided;
            }

            modelValided.Message = $"User {UserName} not is already registered";

            return modelValided;
        }

    }
}
