using RentalMotor.Api.Models;

namespace RentalMotor.Api.Services.Network
{
    public interface IMotorService
    {
        Task<IEnumerable<MotorModel>> GetMotorsAvailableToRental();
    }
}
