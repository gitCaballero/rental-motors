using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Models
{
    public class ModelControllerValidation
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }

        public MotorModel MotorAvailable { get; set; } = new MotorModel();
    }
}
