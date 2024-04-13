using RentalMotor.Api.Models;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IUserMotorService
    {
        IEnumerable<UserMotorModel> GetUsers();
        UserMotorModel GetUserById(string id);
        void AddUser(UserMotorModel user);
        void UpdateUser(UserMotorModel user);
        void DeleteUser(string id);
    }
}
