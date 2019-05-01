using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class questionstudiosingle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudioId",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Question_StudioId",
                table: "Question",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Studio_StudioId",
                table: "Question",
                column: "StudioId",
                principalTable: "Studio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Studio_StudioId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_StudioId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "Question");
        }
    }
}
