using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeHelperApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class NutritionForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeModel");

            migrationBuilder.DropTable(
                name: "DayModel");

            migrationBuilder.DropTable(
                name: "WeekModel");

            migrationBuilder.CreateTable(
                name: "NutritionForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IncludeNutrition = table.Column<bool>(type: "bit", nullable: false),
                    IncludeIngredients = table.Column<bool>(type: "bit", nullable: false),
                    ExcludeIngredients = table.Column<bool>(type: "bit", nullable: false),
                    IncludedIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExcludedIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutrientsJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionForm_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Week",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WeekName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Week", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Week_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekId = table.Column<int>(type: "int", nullable: false),
                    WeekDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCalories = table.Column<double>(type: "float", nullable: false),
                    TotalProtein = table.Column<double>(type: "float", nullable: false),
                    TotalFat = table.Column<double>(type: "float", nullable: false),
                    TotalCarbs = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Day_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<double>(type: "float", nullable: false),
                    Protein = table.Column<double>(type: "float", nullable: false),
                    Fat = table.Column<double>(type: "float", nullable: false),
                    Carbs = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipe_Day_DayId",
                        column: x => x.DayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Day_WeekId",
                table: "Day",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionForm_UserId",
                table: "NutritionForm",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_DayId",
                table: "Recipe",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_UserId",
                table: "Week",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NutritionForm");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "Day");

            migrationBuilder.DropTable(
                name: "Week");

            migrationBuilder.CreateTable(
                name: "WeekModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeekModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekId = table.Column<int>(type: "int", nullable: false),
                    TotalCalories = table.Column<double>(type: "float", nullable: false),
                    TotalCarbs = table.Column<double>(type: "float", nullable: false),
                    TotalFat = table.Column<double>(type: "float", nullable: false),
                    TotalProtein = table.Column<double>(type: "float", nullable: false),
                    WeekDay = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayModel_WeekModel_WeekId",
                        column: x => x.WeekId,
                        principalTable: "WeekModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeModel_DayModel_DayId",
                        column: x => x.DayId,
                        principalTable: "DayModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayModel_WeekId",
                table: "DayModel",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeModel_DayId",
                table: "RecipeModel",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekModel_UserId",
                table: "WeekModel",
                column: "UserId",
                unique: true);
        }
    }
}
