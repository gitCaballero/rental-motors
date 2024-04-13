using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Persistence
{
    public class RentalMotorDbContext : DbContext
    {
        public RentalMotorDbContext(DbContextOptions<RentalMotorDbContext> options) : base(options) { }
        public DbSet<UserMotor> usersMotors { get; set; }
        public DbSet<ContractUserFoorPlan> contractUserFoorPlans { get; set; }
        public DbSet<FoorPlan> foorPlans { get; set; }
    }
}
