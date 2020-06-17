using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class changeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PeriodOfStudy",
                table: "Directions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PeriodOfStudy",
                table: "Directions",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
