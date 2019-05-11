using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class userquestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Question",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_UserId",
                table: "Question",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_User_UserId",
                table: "Question",
                column: "UserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
                */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_Question_User_UserId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_UserId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Question");
                */
        }
    }
}
