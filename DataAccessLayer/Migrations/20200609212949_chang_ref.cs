using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class chang_ref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectScores_Directions_SubjectId",
                table: "SubjectScores");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectScores_EducationalDirectionId",
                table: "SubjectScores",
                column: "EducationalDirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectScores_Directions_EducationalDirectionId",
                table: "SubjectScores",
                column: "EducationalDirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectScores_Directions_EducationalDirectionId",
                table: "SubjectScores");

            migrationBuilder.DropIndex(
                name: "IX_SubjectScores_EducationalDirectionId",
                table: "SubjectScores");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectScores_Directions_SubjectId",
                table: "SubjectScores",
                column: "SubjectId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
