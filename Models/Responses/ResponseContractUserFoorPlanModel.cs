using RentalMotor.Api.Models.Requests;

namespace RentalMotor.Api.Models.Responses
{
    public class ResponseContractUserFoorPlanModel
    {
        public string Id { get; set; }        
        public string StarDate { get; set; }
        public string EndDate { get; set; }
        public decimal PenaltyMissingDaysValue { get; set; }
        public decimal PenaltyOverDaysValue { get; set; }
        public int CountCurrentDays { get; set; }
        public int FloorPlanCountDay { get; set; }
        public string ForecastEndDate { get; set; }
        public string MotorPlate { get; set; }
    }
}
