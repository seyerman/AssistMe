using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class QuestionsLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_Question_QuestionId",
                table: "Label");

            migrationBuilder.DropIndex(
                name: "IX_Label_QuestionId",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Label");

            migrationBuilder.CreateTable(
                name: "QuestionLabels",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false),
                    LabelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLabels", x => new { x.QuestionId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_QuestionLabels_Label_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionLabels_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLabels_LabelId",
                table: "QuestionLabels",
                column: "LabelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionLabels");

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
    }
}
