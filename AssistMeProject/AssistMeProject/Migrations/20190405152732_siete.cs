using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class siete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Label",
                newName: "ID");

            migrationBuilder.CreateTable(
                name: "InterestingVote",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    QuestionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestingVote", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InterestingVote_Question_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestingVote_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PositiveVote",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    AnswerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositiveVote", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PositiveVote_Answer_AnswerID",
                        column: x => x.AnswerID,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PositiveVote_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestingVote_QuestionID",
                table: "InterestingVote",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_InterestingVote_UserID",
                table: "InterestingVote",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PositiveVote_AnswerID",
                table: "PositiveVote",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_PositiveVote_UserID",
                table: "PositiveVote",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestingVote");

            migrationBuilder.DropTable(
                name: "PositiveVote");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Label",
                newName: "Id");
        }
    }
}
