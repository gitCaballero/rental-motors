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

        public RentalUserMotorService(IUserMotorRepository userMotorRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IContractPlanService foorPlanService, IMotorService motorService)
        {
            _foorPlanService = foorPlanService;
            _userMotorRepository = userMotorRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _motorService = motorService;
            UserId = _httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault()!.Value;
            UserName = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Contains("emailaddress")).FirstOrDefault()!.Value;
        }

        public bool AddUser(RequestUserMotorModel? userMotorModel)
        {
            var userMotor = new User();

            try
            {
                userMotor = _mapper.Map<User>(userMotorModel);
                userMotor.UserName = UserName;
                userMotor.UserId = UserId;

                var filePath = Path.Combine(Environment.CurrentDirectory, userMotorModel!.Cnh!.ImagenCnh.FileName);

                using Stream stream = new FileStream(filePath, FileMode.Create);
                userMotorModel.Cnh.ImagenCnh.CopyTo(stream);

                userMotor.Cnh!.ImagePath = filePath;

                _userMotorRepository.Add(userMotor);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool AddContract(MotorModel? motorModel, RequestContractPlanUserMotorModel? contractPlanUserMotorModel)
        {
            var contractUserMotor = new ContractPlanUserMotor();
            var motorContractModel = new MotorContractModel();

            try
            {
                motorContractModel = _mapper.Map<MotorContractModel>(motorModel);

                var flag =  _motorService.UpdateMotorFlag(motorContractModel).Result;

                if (!flag)
                    return false;

                var countDay = contractUserMotor.FloorPlanCountDay;
                var foorPlan = _foorPlanService.GetByCountDay(countDay);
                var forCastEndDate = DateTime.Parse(contractUserMotor.ForecastEndDate);

                var user = BuildContracts.BuildContract(contractPlanUserMotorModel, foorPlan, _mapper);
                user.Id = UserId;

                _userMotorRepository.Update(user);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Delete(string id)
        {
            _userMotorRepository.Delete(id);
        }

        public IEnumerable<ResponseContractUserMotorModel> Get(string? cpfCnpj, string? plate)
        {
            var userMotorModels = new List<ResponseContractUserMotorModel>();
            var usersMotors = _userMotorRepository.Get(cpfCnpj, plate);
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

        public ResponseCnhModel UpdateCnh(IFormFile cnhImage)
        {
            var userId = UserId;
            var userMotor = _userMotorRepository.Get(userId, null).FirstOrDefault();

            var filePath = Path.Combine(Environment.CurrentDirectory, cnhImage.FileName);

            using Stream stream = new FileStream(filePath, FileMode.Create);
            cnhImage.CopyTo(stream);

            userMotor.Cnh!.ImagePath = filePath;

            _userMotorRepository.Update(userMotor);

            userMotor = _userMotorRepository.Get(userId, null).FirstOrDefault();

            var result = _mapper.Map<ResponseCnhModel>(userMotor!.Cnh);
            return result;
        }

        public ModelControllerValidation ValidInputsController(RequestUserMotorModel userMotorModel)
        {
            var modelValided = new ModelControllerValidation();
            modelValided.Message = string.Empty;
            modelValided.IsValid = false;

            var existUser = _userMotorRepository.GetByUserId(UserId);
            if (existUser != null)
            {
                modelValided.Message = $"User {existUser.UserName} is already registered";
                return modelValided;
            }


            var existCpfCnpj = _userMotorRepository.GetByCpfCnpj(userMotorModel.CpfCnpj);
            if (existCpfCnpj != null)
            {
                modelValided.Message = $"Cpf or Cnpj {existCpfCnpj.CpfCnpj} is already registered";
                return modelValided;
            }

            DateTime birthDate;
            var isValidateDate = DateTime.TryParse(userMotorModel.BirthDate, out birthDate);
            if (!isValidateDate)
            {
                modelValided.Message = "BirthDate invalid";
                return modelValided;
            }

            var existCnh = _userMotorRepository.GetCnh(userMotorModel.Cnh!.NumberCnh);
            if (existCnh != null)
            {
                modelValided.Message = $"Cnh {existCnh!.NumberCnh} is already registered";
                return modelValided;
            }


            string pattern = @"^[*a*,b]+$";
            var match = Regex.Match(string.Join(",", userMotorModel.Cnh!.CnhCategories!), pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                modelValided.Message = "You are not qualified to rent a motorcycle or Cnh category invalid";
                return modelValided;
            }

            modelValided.IsValid = true;
            return modelValided;

        }

        public ModelControllerValidation ValidInputsController(IFormFile cnhImage)
        {

            var modelValided = new ModelControllerValidation();
            modelValided.Message = string.Empty;
            modelValided.IsValid = false;

            var existUser = _userMotorRepository.GetByUserId(UserId);
            if (existUser == null)
            {
                modelValided.Message = $"User {existUser.UserName} not already registered";
                return modelValided;
            }

            modelValided.IsValid = true;
            return modelValided;

        }

        public ModelControllerValidation ValidInputsController(RequestContractPlanUserMotorModel contractPlanUserMotorModel)
        {
            var modelValided = new ModelControllerValidation();
            modelValided.Message = string.Empty;
            modelValided.IsValid = false;

            var existUser = _userMotorRepository.GetByUserId(UserId);
            if (existUser != null)
            {
                if (existUser.ContractUserFoorPlan != null)
                {
                    modelValided.Message = $"User {existUser.UserName} is already registered with contract registerd";
                    return modelValided;
                }


                DateTime forecastEndDate;
                var forecastEndValid = DateTime.TryParse(contractPlanUserMotorModel.ForecastEndDate, out forecastEndDate);
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

                var foorPlan = Task.Run(() => _foorPlanService.GetByCountDay(contractPlanUserMotorModel.FloorPlanCountDay));
                if (foorPlan == null)
                {
                    modelValided.Message = "There are no plans for that number of days";
                    return modelValided;
                }

                var motorsAvailable =  _motorService.GetMotorsAvailableToRental().Result;

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
