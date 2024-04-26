using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalMotor.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cnhs_usersMotors_UserMotorId",
                table: "cnhs");

            migrationBuilder.DropForeignKey(
                name: "FK_contractUserFoorPlans_usersMotors_UserMotorId",
                table: "contractUserFoorPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usersMotors",
                table: "usersMotors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_foorPlans",
                table: "foorPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contractUserFoorPlans",
                table: "contractUserFoorPlans");

            migrationBuilder.DropIndex(
                name: "IX_contractUserFoorPlans_UserMotorId",
                table: "contractUserFoorPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cnhs",
                table: "cnhs");

            migrationBuilder.RenameTable(
                name: "usersMotors",
                newName: "UsersMotors");

            migrationBuilder.RenameTable(
                name: "foorPlans",
                newName: "FoorPlans");

            migrationBuilder.RenameTable(
                name: "contractUserFoorPlans",
                newName: "ContractUserFoorPlans");

            migrationBuilder.RenameTable(
                name: "cnhs",
                newName: "Cnhs");

            migrationBuilder.RenameColumn(
                name: "ImagenCnh",
                table: "Cnhs",
                newName: "ImagePath");

            migrationBuilder.RenameIndex(
                name: "IX_cnhs_UserMotorId",
                table: "Cnhs",
                newName: "IX_Cnhs_UserMotorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersMotors",
                table: "UsersMotors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoorPlans",
                table: "FoorPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractUserFoorPlans",
                table: "ContractUserFoorPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cnhs",
                table: "Cnhs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ContractUserFoorPlans_UserMotorId",
                table: "ContractUserFoorPlans",
                column: "UserMotorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cnhs_UsersMotors_UserMotorId",
                table: "Cnhs",
                column: "UserMotorId",
                principalTable: "UsersMotors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUserFoorPlans_UsersMotors_UserMotorId",
                table: "ContractUserFoorPlans",
                column: "UserMotorId",
                principalTable: "UsersMotors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cnhs_UsersMotors_UserMotorId",
                table: "Cnhs");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractUserFoorPlans_UsersMotors_UserMotorId",
                table: "ContractUserFoorPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersMotors",
                table: "UsersMotors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoorPlans",
                table: "FoorPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractUserFoorPlans",
                table: "ContractUserFoorPlans");

            migrationBuilder.DropIndex(
                name: "IX_ContractUserFoorPlans_UserMotorId",
                table: "ContractUserFoorPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cnhs",
                table: "Cnhs");

            migrationBuilder.RenameTable(
                name: "UsersMotors",
                newName: "usersMotors");

            migrationBuilder.RenameTable(
                name: "FoorPlans",
                newName: "foorPlans");

            migrationBuilder.RenameTable(
                name: "ContractUserFoorPlans",
                newName: "contractUserFoorPlans");

            migrationBuilder.RenameTable(
                name: "Cnhs",
                newName: "cnhs");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "cnhs",
                newName: "ImagenCnh");

            migrationBuilder.RenameIndex(
                name: "IX_Cnhs_UserMotorId",
                table: "cnhs",
                newName: "IX_cnhs_UserMotorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usersMotors",
                table: "usersMotors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_foorPlans",
                table: "foorPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contractUserFoorPlans",
                table: "contractUserFoorPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cnhs",
                table: "cnhs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_contractUserFoorPlans_UserMotorId",
                table: "contractUserFoorPlans",
                column: "UserMotorId");

            migrationBuilder.AddForeignKey(
                name: "FK_cnhs_usersMotors_UserMotorId",
                table: "cnhs",
                column: "UserMotorId",
                principalTable: "usersMotors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contractUserFoorPlans_usersMotors_UserMotorId",
                table: "contractUserFoorPlans",
                column: "UserMotorId",
                principalTable: "usersMotors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
