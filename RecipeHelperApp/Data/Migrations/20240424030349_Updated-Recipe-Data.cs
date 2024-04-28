using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeHelperApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRecipeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Carbs",
                table: "Recipes",
                newName: "Sugar");

            migrationBuilder.AddColumn<double>(
                name: "Carbohydrates",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Cholesterol",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fiber",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Potassium",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ServingSize",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Servings",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Sodium",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carbohydrates",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Cholesterol",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Fiber",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Potassium",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ServingSize",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Servings",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Sodium",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "Sugar",
                table: "Recipes",
                newName: "Carbs");
        }
    }
}
