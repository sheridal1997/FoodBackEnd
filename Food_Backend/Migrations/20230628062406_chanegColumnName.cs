using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Food_Backend.Migrations
{
    public partial class chanegColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "UserRating");

            migrationBuilder.AddColumn<int>(
                name: "RecipeRating",
                table: "UserRating",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeRating",
                table: "UserRating");

            migrationBuilder.AddColumn<int>(
                name: "IsFavorite",
                table: "UserRating",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
