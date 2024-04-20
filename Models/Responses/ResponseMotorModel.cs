namespace RentalMotor.Api.Models.Responses
{
    public class ResponseMotorModel : BaseMotorModel
    {
        public string? Identifier { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
    }

    public class BaseMotorModel
    {
        public string? Id { get; set; }
        public string? Plate { get; set; }

    }

    public class MotorModelContract : BaseMotorModel
    {
        public int? IsAvalable { get; set; }
    }

}
