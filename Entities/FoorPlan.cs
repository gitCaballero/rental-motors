namespace RentalMotor.Api.Entities
{
    public class FoorPlan
    {
        public string Id { get; set; }
        public int CostPerDay { get; set; }
        public int CountDay { get; set; }
        public decimal PenaltyPorcent { get; set; }
    }
}
