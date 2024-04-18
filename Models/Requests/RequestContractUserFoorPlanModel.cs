using System.ComponentModel.DataAnnotations;

namespace RentalMotor.Api.Models.Requests
{
    public class RequestContractUserFoorPlanModel
    {
        [Required(ErrorMessage = "FloorPlanCountDay Required")]
        [Display(Name = "FloorPlanCountDay")]
        public int FloorPlanCountDay { get; set; }

        [Required(ErrorMessage = "ForecastEndDate Required")]
        [Display(Name = "ForecastEndDate")]
        public string ForecastEndDate { get; set; }


        [Required(ErrorMessage = "MotorPlate Required")]
        [Display(Name = "MotorPlate")]
        public string MotorPlate { get; set; }
    }
}
