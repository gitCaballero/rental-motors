using System.Web;
namespace RentalMotor.Api.Models.Requests
{
    public class RequestUserMotorModel
    { 
        public required string CpfCnpj { get; set; }

        public required string BirthDate { get; set; }

        public required CnhModel Cnh { get; set; }

    }

    public class CnhModel
    {
        public required List<string> CnhCategories { get; set; }

        public required int NumberCnh { get; set; }

        public required IFormFile ImagenCnh { get; set; }
    }

}
