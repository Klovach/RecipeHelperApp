using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeHelperApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPoundsPerWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetWeightDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TargetWeight",
                table: "AspNetUsers",
                newName: "PoundsPerWeek");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PoundsPerWeek",
                table: "AspNetUsers",
                newName: "TargetWeight");

            migrationBuilder.AddColumn<DateTime>(
                name: "TargetWeightDate",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
