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
        
        public DbSet<Cnh> cnhs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
              .Entity<UserMotor>()
              .Property(e => e.Id)
              .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder
               .Entity<UserMotor>()
               .HasOne(e => e.Cnh)
               .WithOne(e => e.UserMotor)
               .HasForeignKey<Cnh>(e => e.UserMotorId)
               .IsRequired();

            modelBuilder
               .Entity<UserMotor>()
               .HasMany(e => e.ContractUserFoorPlan)
               .WithOne(e => e.UserMotor)
               .HasForeignKey(e => e.UserMotorId)
               .IsRequired();               

            modelBuilder
                .Entity<ContractUserFoorPlan>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder
                .Entity<FoorPlan>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder
                .Entity<Cnh>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
