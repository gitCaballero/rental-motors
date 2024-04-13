using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Mapper;
using RentalMotor.Api.Repository.Implementations;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Repository.Persistence;
using RentalMotor.Api.Services.Implements;
using RentalMotor.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



builder.Services.AddDbContext<RentalMotorDbContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("RentalMotor.Api")));

builder.Services.AddScoped<IUserMotorRepository, UserMotorRepository>(); // Crie a interface e a implementa��o do reposit�rio
builder.Services.AddScoped<IUserMotorService, UserMotorService>(); // Crie a interface e a implementa��o do reposit�rio

builder.Services.AddAutoMapper(typeof(MappingProfile));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
