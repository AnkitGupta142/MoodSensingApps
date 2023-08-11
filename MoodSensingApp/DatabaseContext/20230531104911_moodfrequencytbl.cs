using Microsoft.EntityFrameworkCore.Migrations;

namespace MoodSensingApp.DatabaseContext
{
    public partial class moodfrequencytbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoodFrequencies",
                columns: table => new
                {
                    MoodFrequencyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    Mood = table.Column<string>(nullable: true),
                    Frequency = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoodFrequencies", x => x.MoodFrequencyId);
                    table.ForeignKey(
                        name: "FK_MoodFrequencies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoodFrequencies_UserId",
                table: "MoodFrequencies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoodFrequencies");
        }
    }
}
