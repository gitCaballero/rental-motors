namespace RentalMotor.Api.Entities
{
    public class FoorPlan
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Cost { get; set; }
        public int CountDay { get; set; }
        public int PenaltyPorcent { get; set; }
        public decimal PenaltyValue { get; set; }
        public ContractUserFoorPlan? ContractUserFoorPlan { get; set; }
    }
}
