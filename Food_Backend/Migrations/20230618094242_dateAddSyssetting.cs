using System;
using Food_Backend.Commom;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Food_Backend.Migrations
{
    public partial class dateAddSyssetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ShuffleDate",
                table: "SystemSetting",
                type: "datetime2",
                defaultValue: DateTime.Now.AddDays(1));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShuffleDate",
                table: "SystemSetting");
        }
    }
}
