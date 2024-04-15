using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IUserMotorRepository
    {
        IEnumerable<UserMotor> Get();
        UserMotor GetById(string id);
        void Add(UserMotor user);
        void Update(UserMotor user);
        void Delete(string id);
    }
}
