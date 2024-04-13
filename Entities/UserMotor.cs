namespace RentalMotor.Api.Entities
{
    public class UserMotor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime BirthDate { get; set; }
        public Cnh? Cnh { get; set; }
        public ContractUserFoorPlan? ContractUserFoorPlan{ get; set; }
    }

    public class Cnh
    {
        public string Id { get; set; } = new Guid().ToString();
        public string UserMotorId { get; set; }
        public List<string> CnhCategories { get; set; }
        public int NumberCnh { get; set; }
        public string ImagenCnh { get; set; } = string.Empty;
    }
}
