using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IUserMotorRepository
    {
        IEnumerable<UserMotor> GetUsers();
        UserMotor GetUserById(string id);
        void AddUser(UserMotor user);
        void UpdateUser(UserMotor user);
        void DeleteUser(string id);
    }
}
