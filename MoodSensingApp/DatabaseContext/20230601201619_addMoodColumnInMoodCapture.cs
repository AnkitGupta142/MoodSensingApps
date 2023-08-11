using Microsoft.EntityFrameworkCore.Migrations;

namespace MoodSensingApp.DatabaseContext
{
    public partial class addMoodColumnInMoodCapture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mood",
                table: "MoodCaptures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mood",
                table: "MoodCaptures");
        }
    }
}
