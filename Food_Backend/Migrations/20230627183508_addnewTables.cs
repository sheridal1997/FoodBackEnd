using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Food_Backend.Migrations
{
    public partial class addnewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_RecipeVideo_RecipeVideoId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "RecipeRating",
                table: "Recipe");

            migrationBuilder.CreateTable(
                name: "UserFavorite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: true),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavorite_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: true),
                    IsFavorite = table.Column<int>(type: "int", nullable: false),
                    CreatedUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRating_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorite_Id",
                table: "UserFavorite",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorite_RecipeId",
                table: "UserFavorite",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_Id",
                table: "UserRating",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_RecipeId",
                table: "UserRating",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_RecipeVideo_RecipeVideoId",
                table: "Recipe",
                column: "RecipeVideoId",
                principalTable: "RecipeVideo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_RecipeVideo_RecipeVideoId",
                table: "Recipe");

            migrationBuilder.DropTable(
                name: "UserFavorite");

            migrationBuilder.DropTable(
                name: "UserRating");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Recipe",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecipeRating",
                table: "Recipe",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_RecipeVideo_RecipeVideoId",
                table: "Recipe",
                column: "RecipeVideoId",
                principalTable: "RecipeVideo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
