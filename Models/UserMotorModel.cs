using System.ComponentModel.DataAnnotations;

namespace RentalMotor.Api.Models
{
    public class UserMotorModel
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
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "CnhModel Required")]
        [Display(Name = "CnhModel")]
        public CnhModel? Cnh { get; set; }
    }

    public class CnhModel
    {
        [Required(ErrorMessage = "CnhCategories Required")]
        [Display(Name = "CnhCategories")]
        public List<int> CnhCategories { get; set; }

        [Required(ErrorMessage = "NumberCnh Required")]
        [Display(Name = "NumberCnh")]
        public int NumberCnh { get; set; }

        [Required(ErrorMessage = "ImagenCnh Required")]
        [Display(Name = "ImagenCnh")]
        public string ImagenCnh { get; set; } = string.Empty;
    }
  
}
