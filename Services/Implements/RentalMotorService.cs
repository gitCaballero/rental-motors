using AutoMapper;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Interfaces;

namespace RentalMotor.Api.Services.Implements
{
    public class RentalMotorService : IRentalMotorService
    {
        public readonly IUserMotorRepository _userMotorRepository;
        public readonly IFoorPlanService _foorPlanService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RentalMotorService(IUserMotorRepository userMotorRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFoorPlanService foorPlanService)
        {
            _foorPlanService = foorPlanService;
            _userMotorRepository = userMotorRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Add(RequestUserMotorModel userMotorModel, ref List<ResponseContractUserFoorPlanModel> contractUserFoorPlans)
        {
            try
            {
                var userId = this._httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault()!.Value;
                var userName = this._httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Contains("emailaddress")).FirstOrDefault()!.Value;
              

                var starDate = DateTime.Now.AddDays(1);
                var countDay = userMotorModel.ContractUserFoorPlanModel!.Select(x => x.FloorPlanCountDay).First();
                var forCastEndDate = DateTime.Parse(userMotorModel.ContractUserFoorPlanModel!.Select(x => x.ForecastEndDate).First());
                var endDate = starDate.AddDays(countDay);

                var foorPlan = _foorPlanService.GetByCountDay(countDay);

                var days = forCastEndDate.Subtract(endDate).Days;

                decimal penaltyMissingDaysValue = 0;
                decimal penaltyOverDaysValue = 0;


                if (days < 0 && (foorPlan.CountDay == 7 || foorPlan.CountDay == 15))
                    penaltyMissingDaysValue = ((decimal)(-1 * days * foorPlan.CostPerDay) * (decimal)(foorPlan.PenaltyPorcent / 100)) + (decimal)(-1 * days * foorPlan.CostPerDay);

                if (days > 0)
                    penaltyOverDaysValue = days * 50;


                var userMotor = _mapper.Map<UserMotor>(userMotorModel);

                userMotor.UserName = userName;
                userMotor.UserId = userId;
                userMotor.ContractUserFoorPlan = new List<ContractUserFoorPlan>
               {
                    (new()
                            {
                                EndDate = endDate.ToShortDateString(),
                                FloorPlanCountDay = countDay,
                                ForecastEndDate = forCastEndDate.ToShortDateString(),
                                StarDate = starDate.ToShortDateString(),
                                PenaltyMissingDaysValue = penaltyMissingDaysValue,
                                PenaltyOverDaysValue = penaltyOverDaysValue,
                                MotorPlate = userMotorModel.ContractUserFoorPlanModel!.Select(x => x.MotorPlate).FirstOrDefault()!,
                                CountCurrentDays = days,
                                CostPerDay = foorPlan.CostPerDay,
                                CountDay = foorPlan.CountDay,
                                PenaltyPorcent = foorPlan.PenaltyPorcent,
                                
                            })
                };


                userMotor.Cnh!.CnhCategories = userMotorModel.Cnh!.CnhCategories!;

                _userMotorRepository.Add(userMotor);

                contractUserFoorPlans.Add(_mapper.Map<ResponseContractUserFoorPlanModel>(userMotor.ContractUserFoorPlan.FirstOrDefault()!));

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

        public ResponseUserMotorModel GetById(string id)
        {
            var userMotor = _userMotorRepository.GetById(id);

            var userMotorModel = _mapper.Map<ResponseUserMotorModel>(userMotor);

            return userMotorModel;
        }

        public IEnumerable<ResponseUserMotorModel> Get()
        {
            var userMotorModels = new List<ResponseUserMotorModel>();
            var usersMotors = _userMotorRepository.Get();
            if (usersMotors != null && usersMotors.Any())
            {
                foreach (var userMotor in usersMotors)
                {
                    var userMotorModel = _mapper.Map<ResponseUserMotorModel>(userMotor);

                    foreach (var item in userMotor.ContractUserFoorPlan!)
                    {
                        userMotorModel.ContractUserFoorPlanModel!.Add(new()
                        {
                            EndDate = item.EndDate,
                            FloorPlanCountDay = item.FloorPlanCountDay,
                            ForecastEndDate = item.ForecastEndDate,
                            Id = item.Id,
                            MotorPlate = item.MotorPlate,
                            PenaltyMissingDaysValue = item.PenaltyMissingDaysValue,
                            PenaltyOverDaysValue = item.PenaltyOverDaysValue,
                            StarDate = item.StarDate,
                            CountCurrentDays = item.CountCurrentDays
                        });
                    }
                    userMotorModels.Add(userMotorModel);

                }
                return userMotorModels;
            }
            return userMotorModels;
        }

        public void Update(RequestUserMotorModel userMotorModel)
        {
            var userMotor = _mapper.Map<UserMotor>(userMotorModel);

            _userMotorRepository.Update(userMotor);
        }

    }
}
