using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_PagUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageUrl",
                table: "Divisions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageUrl",
                table: "Directions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageUrl",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "PageUrl",
                table: "Directions");
        }
    }
}
