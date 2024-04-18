using System.ComponentModel.DataAnnotations;

namespace RentalMotor.Api.Models.Requests
{
    public class RequestUserMotorModel
    {

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
        public ICollection<RequestContractUserFoorPlanModel>? ContractUserFoorPlanModel { get; set; }
    }

    public class CnhModel
    {
        [Required(ErrorMessage = "CnhCategories Required")]
        [Display(Name = "CnhCategories")]
        public List<string>? CnhCategories { get; set; }

        [Required(ErrorMessage = "NumberCnh Required")]
        [Display(Name = "NumberCnh")]
        public int NumberCnh { get; set; }

        [Required(ErrorMessage = "ImagenCnh Required")]
        [Display(Name = "ImagenCnh")]
        public string ImagenCnh { get; set; } = string.Empty;
    }

}
