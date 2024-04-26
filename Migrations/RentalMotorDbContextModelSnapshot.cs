﻿// <auto-generated />
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RentalMotor.Api.Repository.Data;

#nullable disable

namespace RentalMotor.Api.Migrations
{
    [DbContext(typeof(ContractPlanUserMotorDbContext))]
    partial class RentalMotorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RentalMotor.Api.Entities.Cnh", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<List<string>>("CnhCategories")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberCnh")
                        .HasColumnType("integer");

                    b.Property<string>("UserMotorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserMotorId")
                        .IsUnique();

                    b.ToTable("Cnhs");
                });

            modelBuilder.Entity("RentalMotor.Api.Entities.ContractPlanUserMotor", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("CostPerDay")
                        .HasColumnType("integer");

                    b.Property<int>("CountCurrentDays")
                        .HasColumnType("integer");

                    b.Property<int>("CountDay")
                        .HasColumnType("integer");

                    b.Property<string>("EndDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FloorPlanCountDay")
                        .HasColumnType("integer");

                    b.Property<string>("ForecastEndDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MotorPlate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("PenaltyMissingDaysValue")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PenaltyOverDaysValue")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PenaltyPorcent")
                        .HasColumnType("numeric");

                    b.Property<string>("StarDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserMotorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserMotorId")
                        .IsUnique();

                    b.ToTable("ContractUserFoorPlans");
                });

            modelBuilder.Entity("RentalMotor.Api.Entities.Plan", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("CostPerDay")
                        .HasColumnType("integer");

                    b.Property<int>("CountDay")
                        .HasColumnType("integer");

                    b.Property<decimal>("PenaltyPorcent")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("FoorPlans");

                    b.HasData(
                        new
                        {
                            Id = "9EE17882-36F6-4E2C-B840-83F44EE05FC4",
                            CostPerDay = 30,
                            CountDay = 7,
                            PenaltyPorcent = 20m
                        },
                        new
                        {
                            Id = "823C8B02-56CB-4EA4-9EAE-C437AB59E738",
                            CostPerDay = 28,
                            CountDay = 15,
                            PenaltyPorcent = 40m
                        },
                        new
                        {
                            Id = "E5200618-67DC-4ECE-9D00-2522D8C71BEA",
                            CostPerDay = 22,
                            CountDay = 30,
                            PenaltyPorcent = 0m
                        },
                        new
                        {
                            Id = "F40BA184-6F7D-44E6-B7A2-15A0D49675EE",
                            CostPerDay = 20,
                            CountDay = 45,
                            PenaltyPorcent = 0m
                        },
                        new
                        {
                            Id = "8CF73A85-1F93-40F4-914A-035B82A01ED4",
                            CostPerDay = 18,
                            CountDay = 50,
                            PenaltyPorcent = 0m
                        });
                });

            modelBuilder.Entity("RentalMotor.Api.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("BirthDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CpfCnpj")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UsersMotors");
                });

            modelBuilder.Entity("RentalMotor.Api.Entities.Cnh", b =>
                {
                    b.HasOne("RentalMotor.Api.Entities.User", "UserMotor")
                        .WithOne("Cnh")
                        .HasForeignKey("RentalMotor.Api.Entities.Cnh", "UserMotorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserMotor");
                });

            modelBuilder.Entity("RentalMotor.Api.Entities.ContractPlanUserMotor", b =>
                {
                    b.HasOne("RentalMotor.Api.Entities.User", "UserMotor")
                        .WithOne("ContractUserFoorPlan")
                        .HasForeignKey("RentalMotor.Api.Entities.ContractPlanUserMotor", "UserMotorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserMotor");
                });

            modelBuilder.Entity("RentalMotor.Api.Entities.User", b =>
                {
                    b.Navigation("Cnh")
                        .IsRequired();

                    b.Navigation("ContractUserFoorPlan");
                });
#pragma warning restore 612, 618
        }
    }
}
