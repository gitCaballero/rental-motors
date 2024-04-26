using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Data ;
using RentalMotor.Api.Repository.Interfaces;

namespace RentalMotor.Api.Repository.Implementations
{
    public class UserMotorRepository : IUserMotorRepository
    {
        private readonly ContractPlanUserMotorDbContext _context;

        public UserMotorRepository(ContractPlanUserMotorDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Get(string ?cpfCnpj, string ?plate)
        {
            if (cpfCnpj != null && plate == null)
                return _context.UsersMotors.Where(x => x.CpfCnpj == cpfCnpj).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);

            if (cpfCnpj == null && plate != null)
                return _context.UsersMotors.Where(x => x.ContractUserFoorPlan!.MotorPlate.Equals(plate, StringComparison.CurrentCultureIgnoreCase)).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);

            if (cpfCnpj != null && plate != null)
                return _context.UsersMotors.Where(x => x.ContractUserFoorPlan!.MotorPlate.Equals(plate, StringComparison.CurrentCultureIgnoreCase) && x.UserId == cpfCnpj).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);

            return _context.UsersMotors.Include(c => c.ContractUserFoorPlan).Include(c => c.Cnh).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);
        }

        public User GetByUserId(string id)
        {
            return _context.UsersMotors.Where(u => u.UserId == id).FirstOrDefault()!;
        }
        
        public User GetByCpfCnpj(string cpfCnpj)
        {
            return _context.UsersMotors.Where(u => u.CpfCnpj == cpfCnpj).FirstOrDefault()!;
        }

        public Cnh GetCnh(int cnhNumber) 
        {
            return _context.Cnhs.Where(x => x.NumberCnh == cnhNumber).FirstOrDefault()!;
        }

        public void Add(User userMotor)
        {
            _context.UsersMotors.Add(userMotor);
            _context.SaveChangesAsync();
        }

        public void Update(User user)
        {
            _context.Update(user);
            _context.SaveChangesAsync();

        }

        public void Delete(string id)
        {
            var user = _context.UsersMotors.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.UsersMotors.Remove(user);
                _context.SaveChangesAsync();
            }
        }
    }
}
