﻿using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RentalMotor.Api.Configurations;
using RentalMotor.Api.Mapper;
using RentalMotor.Api.Repository.Context;
using RentalMotor.Api.Repository.Implementations;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Implements;
using RentalMotor.Api.Services.Interfaces;
using System.Configuration;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMvc();

builder.Services.AddOptions();

builder.Services.AddHttpContextAccessor();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();


builder.Services.Configure<RentalMotorConfig>(options => configuration.GetSection("RentalMotorConfig").Bind(options));



builder.Services.AddDbContext<RentalMotorDbContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("RentalMotor.Api")));

builder.Services.AddScoped<IUserMotorRepository, UserMotorRepository>(); 
builder.Services.AddScoped<IContractUserFoorPlanRepository, ContractUserFoorPlanRepository>(); 
builder.Services.AddScoped<IUserMotorService, UserMotorService>(); 
builder.Services.AddScoped<IMotorService, MotorService>(); 

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
var security = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                };

builder.Services.AddSwaggerGen
  (
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = "Rental Motor Api",
            Version = "v1",
            Description = "Api for Rental Motor",
            Contact = new OpenApiContact() { Name = "David Caballero Savón", Email = "caballero.david2011@gmail.com" },
        });
        c.IncludeXmlComments(xmlPath);
        c.AddSecurityDefinition(
       "Bearer",
       new OpenApiSecurityScheme
       {
           In = ParameterLocation.Header,
           Description = "Copy 'Bearer ' + token'",
           Name = "Authorization",
           Type = SecuritySchemeType.ApiKey,
           BearerFormat = "JWT"
       });

        c.AddSecurityRequirement(security);
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        SaveSigninToken = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();