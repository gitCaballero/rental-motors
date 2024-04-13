namespace RentalMotor.Api.Entities
{
    public class ContractUserFoorPlan
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FoorPlanId{ get; set; }
        public string UserMotorId { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ForecastEndDate { get; set; }
    }
}
