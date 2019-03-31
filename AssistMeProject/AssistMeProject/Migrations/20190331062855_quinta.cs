using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AssistMeProject.Migrations
{
    public partial class quinta : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GOOGLE_KEY = table.Column<string>(nullable: true),
                    LEVEL = table.Column<int>(nullable: false),
                    USERNAME = table.Column<string>(nullable: true),
                    PASSWORD = table.Column<string>(nullable: true),
                    EMAIL = table.Column<string>(nullable: true),
                    PHOTO = table.Column<string>(nullable: true),
                    QUESTIONS_ANSWERED = table.Column<int>(nullable: false),
                    POSITIVE_VOTES_RECEIVED = table.Column<int>(nullable: false),
                    QUESTIONS_ASKED = table.Column<int>(nullable: false),
                    INTERESTING_VOTES_RECEIVED = table.Column<int>(nullable: false),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    INTERESTS_OR_KNOWLEDGE = table.Column<string>(nullable: true),
                    COUNTRY = table.Column<string>(nullable: true),
                    CITY = table.Column<string>(nullable: true),
                    ADMIN = table.Column<Boolean>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
