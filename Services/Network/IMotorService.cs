using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Services.Network
{
    public interface IMotorService
    {
        Task<IEnumerable<MotorModel>> GetMotorsAvailableToRental();
      
        Task<bool> UpdateMotorFlag(MotorContractModel models);
    }
}
