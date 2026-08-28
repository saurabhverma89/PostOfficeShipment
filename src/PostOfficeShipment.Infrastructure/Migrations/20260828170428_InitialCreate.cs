using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PostOfficeShipment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostOffices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostOffices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipmentNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OriginPostOfficeId = table.Column<int>(type: "integer", nullable: false),
                    DestinationPostOfficeId = table.Column<int>(type: "integer", nullable: false),
                    CurrentPostOfficeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ShipmentType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_PostOffices_CurrentPostOfficeId",
                        column: x => x.CurrentPostOfficeId,
                        principalTable: "PostOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipments_PostOffices_DestinationPostOfficeId",
                        column: x => x.DestinationPostOfficeId,
                        principalTable: "PostOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipments_PostOffices_OriginPostOfficeId",
                        column: x => x.OriginPostOfficeId,
                        principalTable: "PostOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipmentId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PostOfficeId = table.Column<int>(type: "integer", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentStatusHistory_PostOffices_PostOfficeId",
                        column: x => x.PostOfficeId,
                        principalTable: "PostOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipmentStatusHistory_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_ZipCode",
                table: "PostOffices",
                column: "ZipCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CurrentPostOfficeId",
                table: "Shipments",
                column: "CurrentPostOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DestinationPostOfficeId",
                table: "Shipments",
                column: "DestinationPostOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OriginPostOfficeId",
                table: "Shipments",
                column: "OriginPostOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipmentNumber",
                table: "Shipments",
                column: "ShipmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentStatusHistory_PostOfficeId",
                table: "ShipmentStatusHistory",
                column: "PostOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentStatusHistory_ShipmentId_ChangedAt",
                table: "ShipmentStatusHistory",
                columns: new[] { "ShipmentId", "ChangedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipmentStatusHistory");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "PostOffices");
        }
    }
}
