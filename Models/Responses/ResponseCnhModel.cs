using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Models.Responses
{
    public class ResponseCnhModel
    {
        public string Id { get; set; }
        public string UserMotorId { get; set; }
        public List<string> CnhCategories { get; set; }
        public int NumberCnh { get; set; }
        public string ImagenCnh { get; set; } = string.Empty;
    }
}
