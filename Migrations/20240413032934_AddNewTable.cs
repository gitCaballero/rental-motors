using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalMotor.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "CnhCategories",
                table: "Cnh",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(int[]),
                oldType: "integer[]");

            migrationBuilder.CreateTable(
                name: "foorPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    CountDay = table.Column<int>(type: "integer", nullable: false),
                    PenaltyPorcent = table.Column<int>(type: "integer", nullable: false),
                    PenaltyValue = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foorPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "contractUserFoorPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FoorPlanId = table.Column<string>(type: "text", nullable: false),
                    UserMotorId = table.Column<string>(type: "text", nullable: false),
                    StarDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ForecastEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contractUserFoorPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contractUserFoorPlans_foorPlans_FoorPlanId",
                        column: x => x.FoorPlanId,
                        principalTable: "foorPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contractUserFoorPlans_usersMotors_UserMotorId",
                        column: x => x.UserMotorId,
                        principalTable: "usersMotors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contractUserFoorPlans_FoorPlanId",
                table: "contractUserFoorPlans",
                column: "FoorPlanId",
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
                name: "contractUserFoorPlans");

            migrationBuilder.DropTable(
                name: "foorPlans");

            migrationBuilder.AlterColumn<int[]>(
                name: "CnhCategories",
                table: "Cnh",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");
        }
    }
}
