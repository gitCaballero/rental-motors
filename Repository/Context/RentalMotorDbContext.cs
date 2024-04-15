using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Context

{
    public class RentalMotorDbContext : DbContext
    {
        public RentalMotorDbContext(DbContextOptions<RentalMotorDbContext> options) : base(options) { }
        public DbSet<UserMotor> usersMotors { get; set; }
        public DbSet<ContractUserFoorPlan> contractUserFoorPlans { get; set; }
        public DbSet<FoorPlan> foorPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserMotor>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            
            modelBuilder
                .Entity<ContractUserFoorPlan>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            
            modelBuilder
                .Entity<FoorPlan>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
