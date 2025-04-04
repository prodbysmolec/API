using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artikelsystem.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InventurFeld : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventuren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bezeichnung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDatum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AbschlussDatum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Bemerkung = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventuren", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventurPositionen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventurId = table.Column<int>(type: "integer", nullable: false),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    SystemMenge = table.Column<int>(type: "integer", nullable: false),
                    GezaehlteMenge = table.Column<int>(type: "integer", nullable: true),
                    IstGeprueft = table.Column<bool>(type: "boolean", nullable: false),
                    DifferenzWert = table.Column<decimal>(type: "numeric", nullable: true),
                    Bemerkung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventurPositionen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventurPositionen_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventurPositionen_Inventuren_InventurId",
                        column: x => x.InventurId,
                        principalTable: "Inventuren",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventurPositionen_ArtikelId",
                table: "InventurPositionen",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_InventurPositionen_InventurId",
                table: "InventurPositionen",
                column: "InventurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventurPositionen");

            migrationBuilder.DropTable(
                name: "Inventuren");
        }
    }
}
