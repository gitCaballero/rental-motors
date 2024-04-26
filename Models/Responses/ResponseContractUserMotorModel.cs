using System.ComponentModel.DataAnnotations;
using RentalMotor.Api.Models.Requests;

namespace RentalMotor.Api.Models.Responses
{
    public class ResponseContractUserMotorModel
    {

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string CpfCnpj { get; set; }

        public string BirthDate { get; set; }

        public CnhModel? Cnh { get; set; }

        public ResponseContractUserFoorPlanModel? ContractUserFoorPlanModel { get; set; }
    }
}
