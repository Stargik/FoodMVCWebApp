using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodMVCWebApp.Migrations
{
    /// <inheritdoc />
    public partial class DishFixingFieldMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuisineCountryTypelId",
                table: "Dishes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CuisineCountryTypelId",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
