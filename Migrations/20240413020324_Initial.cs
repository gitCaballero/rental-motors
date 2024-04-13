using System;
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
                name: "usersMotors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
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
                    CnhCategories = table.Column<int[]>(type: "integer[]", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Cnh_UserMotorId",
                table: "Cnh",
                column: "UserMotorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cnh");

            migrationBuilder.DropTable(
                name: "usersMotors");
        }
    }
}
