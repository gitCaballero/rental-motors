namespace RentalMotor.Api.Entities
{
    public class ContractUserFoorPlan
    {
        public string Id { get; set; }
        public UserMotor UserMotor{ get; set; }
        public int CostPerDay { get; set; }
        public int CountDay { get; set; }
        public decimal PenaltyPorcent { get; set; }
        public string UserMotorId { get; set; }
        public string MotorPlate { get; set; }
        public int FloorPlanCountDay { get; set; }
        public string StarDate { get; set; }
        public string EndDate { get; set; }
        public string ForecastEndDate { get; set; }
        public decimal PenaltyMissingDaysValue { get; set; } 
        public decimal PenaltyOverDaysValue { get; set; }
        public int CountCurrentDays { get; set; }
    }
}
