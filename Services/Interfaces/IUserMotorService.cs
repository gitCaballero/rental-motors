using RentalMotor.Api.Models;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IUserMotorService
    {
        IEnumerable<UserMotorModel> Get();
        UserMotorModel GetById(string id);
        void Add(UserMotorModel user);
        void Update(UserMotorModel user);
        void Delete(string id);
    }
}
