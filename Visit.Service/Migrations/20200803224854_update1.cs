using Microsoft.EntityFrameworkCore.Migrations;

namespace Visit.Service.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BirthLocation",
                table: "AspNetUsers",
                type: "varchar(150)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddColumn<string>(
                name: "ResidenceLocation",
                table: "AspNetUsers",
                type: "varchar(150)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthLocation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResidenceLocation",
                table: "AspNetUsers");
        }
    }
}
