using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Models
{
    public class ModelControllerValidation
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }

        public ResponseMotorModel MotorAvailable { get; set; } = new ResponseMotorModel();
    }
}
