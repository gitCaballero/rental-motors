using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IUserMotorRepository
    {
        IEnumerable<User> Get(string ?id, string ?plate);
        
        User GetByUserId(string id);
        
        void Add(User user);
                
        void Update(User user);
        
        void Delete(string id);

        Cnh GetCnh(int cnhNumber);

        User GetByCpfCnpj(string cpfCnpj);
    }
}
