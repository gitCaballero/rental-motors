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
        private readonly IAwsService _awsService;

        public RentalUserMotorService(IUserMotorRepository userMotorRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IContractPlanService foorPlanService, IMotorService motorService, IAwsService awsService)
        {
            _foorPlanService = foorPlanService;
            _userMotorRepository = userMotorRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _motorService = motorService;
            UserId = _httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault()!.Value;
            UserName = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Contains("emailaddress")).FirstOrDefault()!.Value;
            _awsService = awsService;
        }

        public async Task<ResponseContractUserMotorModel> AddUser(RequestUserMotorModel? userMotorModel)
        {
            try
            {
                var userMotor = _mapper.Map<User>(userMotorModel);
                userMotor.UserName = UserName;
                userMotor.UserId = UserId;

                var resultStatusCode = await _awsService.PutPhotoToAws(userMotor, userMotorModel!.Cnh.ImagenCnh!);
                if (resultStatusCode.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await _awsService.GetPhotoFromAws(UserId!);

                    var user = await Task.Run(() => _userMotorRepository.Add(userMotor));

                    var result = _mapper.Map<ResponseContractUserMotorModel>(user);
                    result.Cnh.ImagePath = response.FirstOrDefault()!.PresignedUrl!;
                    return result;
                }
                return new ();
               
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
                    var countDay = contractPlanUserMotorModel!.FloorPlanCountDay;
                    var foorPlan = _foorPlanService.GetByCountDay(countDay);
                    var forCastEndDate = DateTime.Parse(contractPlanUserMotorModel.ForecastEndDate);

                    var userContract = await Task.Run(() => BuildContracts.BuildContract(contractPlanUserMotorModel!, foorPlan, _mapper));
                    var user = _userMotorRepository.Get(UserId).FirstOrDefault();
                    user!.ContractUserFoorPlan = userContract.ContractUserFoorPlan;

                    var userUpdated = await Task.Run(() => _userMotorRepository.Update(user));

                    var result = _mapper.Map<ResponseContractUserFoorPlanModel>(userUpdated.ContractUserFoorPlan);
                    return result;
                }
                return new();
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
                    var response = await _awsService.GetPhotoFromAws(userMotor.UserId!);
                    userMotorModel.Cnh!.ImagePath = response.FirstOrDefault()!.PresignedUrl!;
                    

                    if (userMotor.ContractUserFoorPlan != null)
                    {
                        var contractModel = _mapper.Map<ResponseContractUserFoorPlanModel>(userMotor.ContractUserFoorPlan);
                        userMotorModel.ContractUserFoorPlanModel = contractModel;
                        userMotorModels.Add(userMotorModel);
                    }
                    else
                        userMotorModels.Add(userMotorModel);
                }
                return userMotorModels;
            }
            return userMotorModels;
        }

        public async Task<ResponseCnhModel> UpdateCnh(IFormFile cnhImage)
        {
            var userMotor = _userMotorRepository.Get(UserId).FirstOrDefault(); 
            var resultStatusCode = await _awsService.PutPhotoToAws(userMotor!, cnhImage);
            if (resultStatusCode.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var response = await _awsService.GetPhotoFromAws(UserId!);

                var user = await Task.Run(() => _userMotorRepository.Update(userMotor));

                var result = _mapper.Map<ResponseContractUserMotorModel>(user);

                result.Cnh.ImagePath = response.FirstOrDefault()!.PresignedUrl!;

                return result.Cnh!;
            }

            return new();
        }

        public async Task<ModelControllerValidation> ValidInputsController(RequestUserMotorModel? userMotorModel = null)
        {
            var modelValided = new ModelControllerValidation
            {
                Message = string.Empty,
                IsValid = false
            };

            var existUser = await Task.Run(() => _userMotorRepository.Get(userId: UserId));
            if (existUser.Any())
            {
                modelValided.Message = $"User {existUser.FirstOrDefault()!.UserName} is already registered";
                return modelValided;
            }

            var existCpfCnpj = await Task.Run(() => _userMotorRepository.Get(cpfCnpj: userMotorModel!.CpfCnpj));
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

            string ext = Path.GetExtension(userMotorModel.Cnh.ImagenCnh.FileName);
            if (!ext.Equals(".png") && !ext.Equals(".bmp"))
            {
                modelValided.Message = "Photo format invalid";
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

            var existUser = await Task.Run(() => _userMotorRepository.Get(userId: UserId));
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
