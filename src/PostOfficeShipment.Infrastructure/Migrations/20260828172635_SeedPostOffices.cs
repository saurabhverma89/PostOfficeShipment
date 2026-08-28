using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PostOfficeShipment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPostOffices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PostOffices",
                columns: new[] { "Id", "Address", "CreatedAt", "Name", "UpdatedAt", "ZipCode" },
                values: new object[,]
                {
                    { 1, "Connaught Place, New Delhi", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), "New Delhi Central", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), "110001" },
                    { 2, "Sector 18, Noida", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Noida Central", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), "201301" },
                    { 3, "MG Road, Gurugram", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Gurugram Central", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), "122001" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PostOffices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PostOffices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PostOffices",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
