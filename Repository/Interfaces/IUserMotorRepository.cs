using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IUserMotorRepository
    {
        IEnumerable<UserMotor> Get(string ?id, string ?plate);
        
        UserMotor GetByUserId(string id);
        
        void Add(UserMotor user);
                
        void Update(UserMotor user);
        
        void Delete(string id);

        Cnh GetCnh(int cnhNumber);

        UserMotor GetByCpfCnpj(string cpfCnpj);
    }
}
