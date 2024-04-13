namespace RentalMotor.Api.Models
{
    public class ContractUserFoorPlanModel
    {
        public FoorPlanModel FoorPlan { get; }
        public UserMotorModel User { get; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ForecastEndDate { get; set; }
    }
}
