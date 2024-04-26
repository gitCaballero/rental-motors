namespace RentalMotor.Api.Entities
{
    public class User
    {
        public string Id { get; set; } 
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CpfCnpj { get; set; }
        public string BirthDate { get; set; }
        public Cnh Cnh { get; set; }
        public ContractPlanUserMotor? ContractUserFoorPlan{ get; set; }
    }

    public class Cnh
    {
        public string Id { get; set; }
        public string  UserMotorId { get; set; }
        public User UserMotor{ get; set; }
        public List<string> CnhCategories { get; set; }
        public int NumberCnh { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
