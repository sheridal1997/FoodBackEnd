using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Food_Backend.Migrations
{
    public partial class add_relation_video_recipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeVideoId",
                table: "Recipe",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_RecipeVideoId",
                table: "Recipe",
                column: "RecipeVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_RecipeVideo_RecipeVideoId",
                table: "Recipe",
                column: "RecipeVideoId",
                principalTable: "RecipeVideo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_RecipeVideo_RecipeVideoId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_RecipeVideoId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "RecipeVideoId",
                table: "Recipe");
        }
    }
}
