using System.ComponentModel.DataAnnotations;

namespace RentalMotor.Api.Models.Requests
{
    public class RequestContractUserFoorPlanModel
    {
        public required int FloorPlanCountDay { get; set; }

        public required string ForecastEndDate { get; set; }

        public required string MotorPlate { get; set; }
    }
}
