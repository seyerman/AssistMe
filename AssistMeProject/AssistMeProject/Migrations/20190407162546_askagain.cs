using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class askagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AskAgain",
                table: "Question",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Label",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Label_QuestionId",
                table: "Label",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Question_QuestionId",
                table: "Label",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_Question_QuestionId",
                table: "Label");

            migrationBuilder.DropIndex(
                name: "IX_Label_QuestionId",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "AskAgain",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Label");
        }
    }
}
