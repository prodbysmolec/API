using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artikelsystem.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Inventur2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SystemMenge",
                table: "InventurPositionen",
                newName: "Menge");

            migrationBuilder.AlterColumn<decimal>(
                name: "DifferenzWert",
                table: "InventurPositionen",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AbschlussDatum",
                table: "Inventuren",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "HistorischGesetzt",
                table: "Artikel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ArtikelInventurHistorie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    InventurId = table.Column<int>(type: "integer", nullable: false),
                    AlteBestandsmenge = table.Column<int>(type: "integer", nullable: false),
                    NeueBestandsmenge = table.Column<int>(type: "integer", nullable: false),
                    Differenz = table.Column<int>(type: "integer", nullable: false),
                    DifferenzWert = table.Column<decimal>(type: "numeric", nullable: false),
                    Datum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikelInventurHistorie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtikelInventurHistorie_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtikelInventurHistorie_Inventuren_InventurId",
                        column: x => x.InventurId,
                        principalTable: "Inventuren",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventurBerichte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventurId = table.Column<int>(type: "integer", nullable: false),
                    Titel = table.Column<string>(type: "text", nullable: false),
                    Inhalt = table.Column<string>(type: "text", nullable: false),
                    Erstellungsdatum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GesamtDifferenzWert = table.Column<decimal>(type: "numeric", nullable: false),
                    AnzahlPositionenMitDifferenz = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventurBerichte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventurBerichte_Inventuren_InventurId",
                        column: x => x.InventurId,
                        principalTable: "Inventuren",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelInventurHistorie_ArtikelId",
                table: "ArtikelInventurHistorie",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelInventurHistorie_InventurId",
                table: "ArtikelInventurHistorie",
                column: "InventurId");

            migrationBuilder.CreateIndex(
                name: "IX_InventurBerichte_InventurId",
                table: "InventurBerichte",
                column: "InventurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtikelInventurHistorie");

            migrationBuilder.DropTable(
                name: "InventurBerichte");

            migrationBuilder.DropColumn(
                name: "HistorischGesetzt",
                table: "Artikel");

            migrationBuilder.RenameColumn(
                name: "Menge",
                table: "InventurPositionen",
                newName: "SystemMenge");

            migrationBuilder.AlterColumn<decimal>(
                name: "DifferenzWert",
                table: "InventurPositionen",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AbschlussDatum",
                table: "Inventuren",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
