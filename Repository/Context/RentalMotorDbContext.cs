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

            modelBuilder.Entity<FoorPlan>().HasData(

                new FoorPlan { Id = "9EE17882-36F6-4E2C-B840-83F44EE05FC4", CostPerDay = 30, CountDay = 7, PenaltyPorcent = 20 },
                new FoorPlan { Id = "823C8B02-56CB-4EA4-9EAE-C437AB59E738", CostPerDay = 28, CountDay = 15, PenaltyPorcent = 40 },
                new FoorPlan { Id = "E5200618-67DC-4ECE-9D00-2522D8C71BEA", CostPerDay = 22, CountDay = 30, PenaltyPorcent = 0 },
                new FoorPlan { Id = "F40BA184-6F7D-44E6-B7A2-15A0D49675EE", CostPerDay = 20, CountDay = 45, PenaltyPorcent = 0 },
                new FoorPlan { Id = "8CF73A85-1F93-40F4-914A-035B82A01ED4", CostPerDay = 18, CountDay = 50, PenaltyPorcent = 0 });

            modelBuilder
                .Entity<Cnh>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
