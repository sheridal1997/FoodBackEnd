using Food_Backend.Controllers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Food_Backend.Migrations
{
    public partial class addUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [User] (firstName, lastName, userName,password ,UserType,IsDelete) values ('Bilal','lahri','bilallehri','YmlsYWwxMjM0Z0RQMUgyMXFNYQ==','1','0')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
