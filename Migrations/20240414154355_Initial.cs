using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalMotor.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "foorPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    CountDay = table.Column<int>(type: "integer", nullable: false),
                    PenaltyPorcent = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foorPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usersMotors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    CpfCnpj = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersMotors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cnh",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserMotorId = table.Column<string>(type: "text", nullable: false),
                    CnhCategories = table.Column<List<string>>(type: "text[]", nullable: false),
                    NumberCnh = table.Column<int>(type: "integer", nullable: false),
                    ImagenCnh = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cnh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cnh_usersMotors_UserMotorId",
                        column: x => x.UserMotorId,
                        principalTable: "usersMotors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contractUserFoorPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserMotorId = table.Column<string>(type: "text", nullable: false),
                    FoorPlanId = table.Column<string>(type: "text", nullable: false),
                    StarDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ForecastEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PenaltyValue = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contractUserFoorPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contractUserFoorPlans_usersMotors_UserMotorId",
                        column: x => x.UserMotorId,
                        principalTable: "usersMotors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cnh_UserMotorId",
                table: "Cnh",
                column: "UserMotorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contractUserFoorPlans_UserMotorId",
                table: "contractUserFoorPlans",
                column: "UserMotorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cnh");

            migrationBuilder.DropTable(
                name: "contractUserFoorPlans");

            migrationBuilder.DropTable(
                name: "foorPlans");

            migrationBuilder.DropTable(
                name: "usersMotors");
        }
    }
}
