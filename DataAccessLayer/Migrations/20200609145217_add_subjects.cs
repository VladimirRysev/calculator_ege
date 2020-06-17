using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_subjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreateDateTime", "ModifiedDateTime", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Физика" },
                    { 2, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Химия" },
                    { 3, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Русский язык" },
                    { 4, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Математика (профильная)" },
                    { 5, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Биология" },
                    { 6, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Творческий конкурс" },
                    { 7, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Спортивная дисциплина" },
                    { 8, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "География" },
                    { 9, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Обществознание" },
                    { 10, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Литература" },
                    { 11, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "История" },
                    { 12, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Информатика" },
                    { 13, new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), new DateTime(2020, 6, 9, 17, 52, 17, 269, DateTimeKind.Local).AddTicks(1428), "Иностранный язык" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
