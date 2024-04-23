using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Services.Network
{
    public interface IMotorService
    {
        Task<IEnumerable<ResponseMotorModel>> GetMotorsAvailableToRental();
      
        Task<bool> UpdateMotorFlag(MotorModelContract models);
    }
}
