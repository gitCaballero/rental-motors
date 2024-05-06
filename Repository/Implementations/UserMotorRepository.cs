using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Data;
using RentalMotor.Api.Repository.Interfaces;

namespace RentalMotor.Api.Repository.Implementations
{
    public class UserMotorRepository(ContractPlanUserMotorDbContext context) : IUserMotorRepository
    {
        private readonly ContractPlanUserMotorDbContext _context = context;

        public IEnumerable<User> Get(string? userId = null, string? cpfCnpj = null, string? plate = null)
        {
            if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(cpfCnpj) && string.IsNullOrEmpty(plate))
                return _context.UsersMotors.Where(x => x.UserId.Equals(userId)).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);


            if (string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(cpfCnpj) && string.IsNullOrEmpty(plate))
                return _context.UsersMotors.Where(x => x.CpfCnpj.ToUpper().Equals(cpfCnpj.ToUpper())).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);


            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(cpfCnpj) && !string.IsNullOrEmpty(plate))
                return _context.UsersMotors.Where(x => x.ContractUserFoorPlan!.MotorPlate.ToUpper().Equals(plate.ToUpper())).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);


            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(cpfCnpj) && !string.IsNullOrEmpty(plate))
                return _context.UsersMotors.Where(x => (x.UserId.Equals(userId)) &&
                    (x.CpfCnpj.ToUpper().Equals(cpfCnpj.ToUpper())) &&
                    (x.ContractUserFoorPlan!.MotorPlate.ToUpper().Equals(plate.ToUpper()))).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);

            return _context.UsersMotors.Include(c => c.ContractUserFoorPlan).Include(c => c.Cnh).Include(h => h.Cnh).Include(c => c.ContractUserFoorPlan);
        }

        public Cnh GetCnh(int cnhNumber)
        {
            return _context.Cnhs.Where(x => x.NumberCnh == cnhNumber).FirstOrDefault()!;
        }

        public User Add(User userMotor)
        {
            _context.UsersMotors.Add(userMotor);
            _ = _context.SaveChangesAsync().Result;
            return userMotor;
        }

        public User Update(User user)
        {
            _context.Update(user);
            _ = _context.SaveChangesAsync().Result;
            return user;

        }

        public bool Delete(string id)
        {
            var user = _context.UsersMotors.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                _context.UsersMotors.Remove(user);
                _ = _context.SaveChangesAsync().Result;
                return true;
            }
            return false;
        }
    }
}
