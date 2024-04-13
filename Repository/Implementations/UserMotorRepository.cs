using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Repository.Persistence;

namespace RentalMotor.Api.Repository.Implementations
{
    public class UserMotorRepository: IUserMotorRepository
    {
        private readonly RentalMotorDbContext _context;

        public UserMotorRepository(RentalMotorDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserMotor> GetUsers()
        {
            return _context.usersMotors;
        }

        public UserMotor GetUserById(string id)
        {
            return _context.usersMotors.Where(u => u.Id == id).FirstOrDefault()!;          
        }

        public void AddUser(UserMotor userMotor)
        {
            _context.usersMotors.Add(userMotor);
            _context.SaveChanges();
        }

        public void UpdateUser(UserMotor user)
        {
            var existingUser = _context.usersMotors.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                _context.Update(user);
                _context.SaveChanges();
            }
        }

        public void DeleteUser(string id)
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
