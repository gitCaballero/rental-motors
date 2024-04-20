using AutoMapper;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models.Requests;

namespace RentalMotor.Api.Services.Responsabilities
{
    public static class BuildContracts
    {

        public static UserMotor BuildContract(RequestUserMotorModel userMotorModel, FoorPlan foorPlan, IMapper _mapper)
        {

            var countDay = userMotorModel.ContractUserFoorPlanModel!.First().FloorPlanCountDay;
            var forCastEndDate = DateTime.Parse(userMotorModel.ContractUserFoorPlanModel!.First().ForecastEndDate);

            var starDate = DateTime.Now.AddDays(1);
            var endDate = starDate.AddDays(countDay);

            var days = forCastEndDate.Subtract(endDate).Days;

            decimal penaltyMissingDaysValue = 0;
            decimal penaltyOverDaysValue = 0;

            if (days < 0 && (foorPlan.CountDay == 7 || foorPlan.CountDay == 15))
                penaltyMissingDaysValue = ((decimal)(-1 * days * foorPlan.CostPerDay) * (decimal)(foorPlan.PenaltyPorcent / 100)) + (decimal)(-1 * days * foorPlan.CostPerDay);

            if (days > 0)
                penaltyOverDaysValue = days * 50;


            var userMotor = _mapper.Map<UserMotor>(userMotorModel);


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
            return userMotor;
        }
    }
}
