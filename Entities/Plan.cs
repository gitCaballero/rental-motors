namespace RentalMotor.Api.Entities
{
    public class Plan
    {
        public string Id { get; set; }
        public int CostPerDay { get; set; }
        public int CountDay { get; set; }
        public decimal PenaltyPorcent { get; set; }
    }
}
