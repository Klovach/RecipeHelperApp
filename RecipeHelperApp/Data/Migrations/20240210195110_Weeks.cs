using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeHelperApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Weeks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Day_Week_WeekId",
                table: "Day");

            migrationBuilder.DropForeignKey(
                name: "FK_Week_AspNetUsers_UserId",
                table: "Week");

            migrationBuilder.DropIndex(
                name: "IX_Week_UserId",
                table: "Week");

            migrationBuilder.AlterColumn<string>(
                name: "WeekName",
                table: "Week",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Week",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Week",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NutrientsJson",
                table: "NutritionForm",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Day",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "WeekDay",
                table: "Day",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Week_UserId",
                table: "Week",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Day_Week_WeekId",
                table: "Day",
                column: "WeekId",
                principalTable: "Week",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Week_AspNetUsers_UserId",
                table: "Week",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Day_Week_WeekId",
                table: "Day");

            migrationBuilder.DropForeignKey(
                name: "FK_Week_AspNetUsers_UserId",
                table: "Week");

            migrationBuilder.DropIndex(
                name: "IX_Week_UserId",
                table: "Week");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Week");

            migrationBuilder.AlterColumn<string>(
                name: "WeekName",
                table: "Week",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Week",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NutrientsJson",
                table: "NutritionForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Day",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WeekDay",
                table: "Day",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Week_UserId",
                table: "Week",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Day_Week_WeekId",
                table: "Day",
                column: "WeekId",
                principalTable: "Week",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Week_AspNetUsers_UserId",
                table: "Week",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
