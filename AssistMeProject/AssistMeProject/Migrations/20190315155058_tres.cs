using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class tres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Answer");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Question",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Question",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Answer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Question",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Question",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "Question",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Question",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "Comment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Comment",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Answer",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "Answer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Answer",
                nullable: true);
        }
    }
}
