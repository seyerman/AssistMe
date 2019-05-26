using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class tagslimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Question",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30000);

            migrationBuilder.AddColumn<string>(
                name: "question_tags",
                table: "Question",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comment",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30000);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Answer",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "question_tags",
                table: "Question");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Question",
                maxLength: 30000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comment",
                maxLength: 30000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Answer",
                maxLength: 30000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);
        }
    }
}
