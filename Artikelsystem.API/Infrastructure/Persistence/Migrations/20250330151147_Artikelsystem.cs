using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artikelsystem.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Artikelsystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artikel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Preis = table.Column<decimal>(type: "numeric", nullable: false),
                    Maximalbestand = table.Column<int>(type: "integer", nullable: false),
                    Mindestbestand = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Bild = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lieferanten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Firma = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Vorname = table.Column<string>(type: "text", nullable: false),
                    EmailAdresse = table.Column<string>(type: "text", nullable: false),
                    Strasse = table.Column<string>(type: "text", nullable: false),
                    Hausnummer = table.Column<string>(type: "text", nullable: false),
                    PLZ = table.Column<string>(type: "text", nullable: false),
                    Ort = table.Column<string>(type: "text", nullable: false),
                    Telefonnummer = table.Column<string>(type: "text", nullable: false),
                    Notizen = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lieferanten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtikelStatistiken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    Gesamtmenge = table.Column<decimal>(type: "numeric", nullable: false),
                    DurchschnittlicherEinzelpreis = table.Column<decimal>(type: "numeric", nullable: false),
                    DurchschnittlicherVerkaufspreis = table.Column<decimal>(type: "numeric", nullable: false),
                    VerkaufsMenge = table.Column<int>(type: "integer", nullable: false),
                    Lagerwert = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "\"Gesamtmenge\" * \"DurchschnittlicherEinzelpreis\"", stored: true),
                    GesamtVerkaufswert = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "\"VerkaufsMenge\" * \"DurchschnittlicherVerkaufspreis\"", stored: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikelStatistiken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtikelStatistiken_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wareneingaenge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LieferantId = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Gesamtpreis = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wareneingaenge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wareneingaenge_Lieferanten_LieferantId",
                        column: x => x.LieferantId,
                        principalTable: "Lieferanten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WareneingangArtikel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    WareneingangId = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Einzelpreis = table.Column<decimal>(type: "numeric", nullable: false),
                    Gesamtpreis = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "\"Menge\" * \"Einzelpreis\"", stored: true)
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
                name: "IX_ArtikelStatistiken_ArtikelId",
                table: "ArtikelStatistiken",
                column: "ArtikelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wareneingaenge_LieferantId",
                table: "Wareneingaenge",
                column: "LieferantId");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikel_ArtikelId",
                table: "WareneingangArtikel",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikel_WareneingangId",
                table: "WareneingangArtikel",
                column: "WareneingangId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtikelStatistiken");

            migrationBuilder.DropTable(
                name: "WareneingangArtikel");

            migrationBuilder.DropTable(
                name: "Artikel");

            migrationBuilder.DropTable(
                name: "Wareneingaenge");

            migrationBuilder.DropTable(
                name: "Lieferanten");
        }
    }
}
