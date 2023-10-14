using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Food_Backend.Migrations
{
    public partial class add_End_Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "WeeklyPlan",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "WeeklyPlan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WeeklyPlan",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "WeeklyPlan");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WeeklyPlan");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "WeeklyPlan",
                newName: "Date");
        }
    }
}
