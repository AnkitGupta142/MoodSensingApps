using Microsoft.EntityFrameworkCore.Migrations;

namespace MoodSensingApp.DatabaseContext
{
    public partial class addCityColumnInLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Locations");
        }
    }
}
