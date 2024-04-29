using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IUserMotorRepository
    {
        IEnumerable<User> Get(string ?userId = null, string? cpfCnpj = null, string? plate = null);
        
        User Add(User user);
                
        User Update(User user);
        
        bool Delete(string id);

        Cnh GetCnh(int cnhNumber);
    }
}
