using System.ComponentModel.DataAnnotations;
using RentalMotor.Api.Models.Requests;

namespace RentalMotor.Api.Models.Responses
{
    public class ResponseContractUserMotorModel
    {
        [Required(ErrorMessage = "UserId Required")]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "UserName Required")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "CpfCnpj Required")]
        [Display(Name = "CpfCnpj")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "BirthDate Required")]
        [Display(Name = "BirthDate")]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "CnhModel Required")]
        [Display(Name = "CnhModel")]
        public CnhModel? Cnh { get; set; }

        [Required(ErrorMessage = "ContractUserFoorPlanModel Required")]
        [Display(Name = "ContractUserFoorPlanModel")]
        public ICollection<ResponseContractUserFoorPlanModel>? ContractUserFoorPlanModel { get; set; } = new List<ResponseContractUserFoorPlanModel>();
    }
}
