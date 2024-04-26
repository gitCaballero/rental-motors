using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Repository.Data;

namespace RentalMotor.Api.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ContractPlanUserMotorDbContext dbContext = scope.ServiceProvider.GetRequiredService<ContractPlanUserMotorDbContext>();

            dbContext.Database.Migrate();
        }
    }
}

