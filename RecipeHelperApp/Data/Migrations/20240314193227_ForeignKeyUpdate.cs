using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeHelperApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Day_Week_WeekId",
                table: "Day");

            migrationBuilder.DropForeignKey(
                name: "FK_NutritionForm_AspNetUsers_UserId",
                table: "NutritionForm");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Day_DayId",
                table: "Recipe");

            migrationBuilder.DropForeignKey(
                name: "FK_Week_AspNetUsers_UserId",
                table: "Week");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Week",
                table: "Week");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NutritionForm",
                table: "NutritionForm");

            migrationBuilder.DropIndex(
                name: "IX_NutritionForm_UserId",
                table: "NutritionForm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Day",
                table: "Day");

            migrationBuilder.RenameTable(
                name: "Week",
                newName: "Weeks");

            migrationBuilder.RenameTable(
                name: "Recipe",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "NutritionForm",
                newName: "NutritionForms");

            migrationBuilder.RenameTable(
                name: "Day",
                newName: "Days");

            migrationBuilder.RenameIndex(
                name: "IX_Week_UserId",
                table: "Weeks",
                newName: "IX_Weeks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_DayId",
                table: "Recipes",
                newName: "IX_Recipes_DayId");

            migrationBuilder.RenameIndex(
                name: "IX_Day_WeekId",
                table: "Days",
                newName: "IX_Days_WeekId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Weeks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NutritionForms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Days",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weeks",
                table: "Weeks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NutritionForms",
                table: "NutritionForms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Days",
                table: "Days",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionForms_UserId",
                table: "NutritionForms",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Weeks_WeekId",
                table: "Days",
                column: "WeekId",
                principalTable: "Weeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionForms_AspNetUsers_UserId",
                table: "NutritionForms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Days_DayId",
                table: "Recipes",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weeks_AspNetUsers_UserId",
                table: "Weeks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Weeks_WeekId",
                table: "Days");

            migrationBuilder.DropForeignKey(
                name: "FK_NutritionForms_AspNetUsers_UserId",
                table: "NutritionForms");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Days_DayId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Weeks_AspNetUsers_UserId",
                table: "Weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weeks",
                table: "Weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NutritionForms",
                table: "NutritionForms");

            migrationBuilder.DropIndex(
                name: "IX_NutritionForms_UserId",
                table: "NutritionForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Days",
                table: "Days");

            migrationBuilder.RenameTable(
                name: "Weeks",
                newName: "Week");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipe");

            migrationBuilder.RenameTable(
                name: "NutritionForms",
                newName: "NutritionForm");

            migrationBuilder.RenameTable(
                name: "Days",
                newName: "Day");

            migrationBuilder.RenameIndex(
                name: "IX_Weeks_UserId",
                table: "Week",
                newName: "IX_Week_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_DayId",
                table: "Recipe",
                newName: "IX_Recipe_DayId");

            migrationBuilder.RenameIndex(
                name: "IX_Days_WeekId",
                table: "Day",
                newName: "IX_Day_WeekId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Week",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Recipe",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NutritionForm",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Day",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Week",
                table: "Week",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NutritionForm",
                table: "NutritionForm",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Day",
                table: "Day",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionForm_UserId",
                table: "NutritionForm",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Day_Week_WeekId",
                table: "Day",
                column: "WeekId",
                principalTable: "Week",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionForm_AspNetUsers_UserId",
                table: "NutritionForm",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Day_DayId",
                table: "Recipe",
                column: "DayId",
                principalTable: "Day",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Week_AspNetUsers_UserId",
                table: "Week",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
