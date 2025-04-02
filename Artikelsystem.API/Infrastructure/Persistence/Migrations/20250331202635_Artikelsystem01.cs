using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artikelsystem.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Artikelsystem01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wareneingaenge_Lieferanten_LieferantId",
                table: "Wareneingaenge");

            migrationBuilder.DropTable(
                name: "WareneingangArtikel");

            migrationBuilder.DropIndex(
                name: "IX_Wareneingaenge_LieferantId",
                table: "Wareneingaenge");

            migrationBuilder.DropColumn(
                name: "LieferantId",
                table: "Wareneingaenge");

            migrationBuilder.DropColumn(
                name: "Menge",
                table: "Wareneingaenge");

            migrationBuilder.AlterColumn<decimal>(
                name: "Gesamtpreis",
                table: "Wareneingaenge",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "AllgemeineBemerkungen",
                table: "Wareneingaenge",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Warenausgaenge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mitarbeiter = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AllgemeineBemerkungen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warenausgaenge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareneingangArtikelPositionen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    WareneingangId = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Einzelpreis = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "\"Menge\" * \"Gesamtpreis\"", stored: true),
                    Gesamtpreis = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareneingangArtikelPositionen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareneingangArtikelPositionen_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WareneingangArtikelPositionen_Wareneingaenge_WareneingangId",
                        column: x => x.WareneingangId,
                        principalTable: "Wareneingaenge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarenausgangArtikelPositionen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WarenausgangId = table.Column<int>(type: "integer", nullable: false),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    Zweck = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Bemerkung = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Verkaufspreis = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Rechnungsnummer = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Gesamtpreis = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarenausgangArtikelPositionen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarenausgangArtikelPositionen_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarenausgangArtikelPositionen_Warenausgaenge_WarenausgangId",
                        column: x => x.WarenausgangId,
                        principalTable: "Warenausgaenge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarenausgangArtikelPositionen_ArtikelId",
                table: "WarenausgangArtikelPositionen",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_WarenausgangArtikelPositionen_WarenausgangId",
                table: "WarenausgangArtikelPositionen",
                column: "WarenausgangId");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikelPositionen_ArtikelId",
                table: "WareneingangArtikelPositionen",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikelPositionen_WareneingangId_ArtikelId",
                table: "WareneingangArtikelPositionen",
                columns: new[] { "WareneingangId", "ArtikelId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarenausgangArtikelPositionen");

            migrationBuilder.DropTable(
                name: "WareneingangArtikelPositionen");

            migrationBuilder.DropTable(
                name: "Warenausgaenge");

            migrationBuilder.DropColumn(
                name: "AllgemeineBemerkungen",
                table: "Wareneingaenge");

            migrationBuilder.AlterColumn<decimal>(
                name: "Gesamtpreis",
                table: "Wareneingaenge",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "LieferantId",
                table: "Wareneingaenge",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Menge",
                table: "Wareneingaenge",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WareneingangArtikel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    WareneingangId = table.Column<int>(type: "integer", nullable: false),
                    Einzelpreis = table.Column<decimal>(type: "numeric", nullable: false),
                    Gesamtpreis = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "\"Menge\" * \"Einzelpreis\"", stored: true),
                    Menge = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareneingangArtikel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareneingangArtikel_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WareneingangArtikel_Wareneingaenge_WareneingangId",
                        column: x => x.WareneingangId,
                        principalTable: "Wareneingaenge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wareneingaenge_LieferantId",
                table: "Wareneingaenge",
                column: "LieferantId");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikel_ArtikelId",
                table: "WareneingangArtikel",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikel_WareneingangId_ArtikelId",
                table: "WareneingangArtikel",
                columns: new[] { "WareneingangId", "ArtikelId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wareneingaenge_Lieferanten_LieferantId",
                table: "Wareneingaenge",
                column: "LieferantId",
                principalTable: "Lieferanten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
