using RentalMotors.MessageBus;

namespace RentalMotor.Api.Models.Responses
{
    public class ResponseContractUserMotorModel : BaseMessage
    {

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string CpfCnpj { get; set; }

        public string BirthDate { get; set; }

        public ResponseCnhModel? Cnh { get; set; }

        public ResponseContractUserFoorPlanModel? ContractUserFoorPlanModel { get; set; }
    }
}
