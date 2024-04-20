using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Context;
using RentalMotor.Api.Repository.Interfaces;

namespace RentalMotor.Api.Repository.Implementations
{
    public class UserMotorRepository : IUserMotorRepository
    {
        private readonly RentalMotorDbContext _context;

        public UserMotorRepository(RentalMotorDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserMotor> Get()
        {
            return _context.usersMotors.Include(x => x.ContractUserFoorPlan).Include(c => c.Cnh);
        }

        public UserMotor GetById(string id)
        {
            return _context.usersMotors.Where(u => u.Id == id).FirstOrDefault()!;
        }
        
        public UserMotor GetByCpfCnpj(string cpfCnpj)
        {
            return _context.usersMotors.Where(u => u.CpfCnpj == cpfCnpj).FirstOrDefault()!;
        }

        public Cnh GetCnh(int cnhNumber) 
        {
            return _context.cnhs.Where(x => x.NumberCnh == cnhNumber).FirstOrDefault()!;
        }

        public void Add(UserMotor userMotor)
        {
            _context.usersMotors.Add(userMotor);
            _context.SaveChanges();
        }

        public void Update(UserMotor user)
        {
            _context.Update(user);
            _context.SaveChanges();

        }

        public void Delete(string id)
        {
            var user = _context.usersMotors.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.usersMotors.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
