using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class questionRepeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Insignia",
                table: "Question",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlOriginalQuestion",
                table: "Answer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Insignia",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "UrlOriginalQuestion",
                table: "Answer");
        }
    }
}
