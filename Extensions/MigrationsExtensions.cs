using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Repository.Context;

namespace RentalMotor.Api.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using RentalMotorDbContext dbContext = scope.ServiceProvider.GetRequiredService<RentalMotorDbContext>();

            dbContext.Database.Migrate();
        }
    }
}

