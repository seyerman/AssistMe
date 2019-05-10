using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class studiousers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "StudioId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_StudioId",
                table: "User",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Studio_StudioId",
                table: "User",
                column: "StudioId",
                principalTable: "Studio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Studio_StudioId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_StudioId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Question",
                nullable: true);
        }
    }
}
