using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeHelperApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class MealPlanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    WeekDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCalories = table.Column<double>(type: "float", nullable: false),
                    TotalProtein = table.Column<double>(type: "float", nullable: false),
                    TotalFat = table.Column<double>(type: "float", nullable: false),
                    TotalCarbs = table.Column<double>(type: "float", nullable: false)
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
                    ResultId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeModel");

            migrationBuilder.DropTable(
                name: "DayModel");

            migrationBuilder.DropTable(
                name: "WeekModel");
        }
    }
}
