using RentalMotor.Api.Models;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IMotorService
    {
        Task<MotorModel> GetMotorsAvalableToRental();
    }
}
