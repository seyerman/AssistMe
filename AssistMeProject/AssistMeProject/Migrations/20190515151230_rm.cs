using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssistMeProject.Migrations
{
    public partial class rm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            

           

            
              

            


           

            migrationBuilder.CreateTable(
                name: "QuestionStudio",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false),
                    StudioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionStudio", x => new { x.QuestionId, x.StudioId });
                    table.ForeignKey(
                        name: "FK_QuestionStudio_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionStudio_Studio_StudioId",
                        column: x => x.StudioId,
                        principalTable: "Studio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });



        

           

           

          

        

         

          

        


          


            migrationBuilder.CreateIndex(
                name: "IX_QuestionStudio_StudioId",
                table: "QuestionStudio",
                column: "StudioId");


          

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         

       

         

           

            migrationBuilder.DropTable(
                name: "QuestionStudio");

           
        }
    }
}
