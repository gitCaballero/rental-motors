using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalMotor.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "foorPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CostPerDay = table.Column<int>(type: "integer", nullable: false),
                    CountDay = table.Column<int>(type: "integer", nullable: false),
                    PenaltyPorcent = table.Column<decimal>(type: "numeric", nullable: false)
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
                    BirthDate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersMotors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cnhs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserMotorId = table.Column<string>(type: "text", nullable: false),
                    CnhCategories = table.Column<List<string>>(type: "text[]", nullable: false),
                    NumberCnh = table.Column<int>(type: "integer", nullable: false),
                    ImagenCnh = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cnhs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cnhs_usersMotors_UserMotorId",
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
                    CostPerDay = table.Column<int>(type: "integer", nullable: false),
                    CountDay = table.Column<int>(type: "integer", nullable: false),
                    PenaltyPorcent = table.Column<decimal>(type: "numeric", nullable: false),
                    UserMotorId = table.Column<string>(type: "text", nullable: false),
                    MotorPlate = table.Column<string>(type: "text", nullable: false),
                    FloorPlanCountDay = table.Column<int>(type: "integer", nullable: false),
                    StarDate = table.Column<string>(type: "text", nullable: false),
                    EndDate = table.Column<string>(type: "text", nullable: false),
                    ForecastEndDate = table.Column<string>(type: "text", nullable: false),
                    PenaltyMissingDaysValue = table.Column<decimal>(type: "numeric", nullable: false),
                    PenaltyOverDaysValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CountCurrentDays = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_cnhs_UserMotorId",
                table: "cnhs",
                column: "UserMotorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contractUserFoorPlans_UserMotorId",
                table: "contractUserFoorPlans",
                column: "UserMotorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cnhs");

            migrationBuilder.DropTable(
                name: "contractUserFoorPlans");

            migrationBuilder.DropTable(
                name: "foorPlans");

            migrationBuilder.DropTable(
                name: "usersMotors");
        }
    }
}
