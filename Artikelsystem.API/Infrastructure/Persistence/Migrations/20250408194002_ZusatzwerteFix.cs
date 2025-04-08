using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artikelsystem.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ZusatzwerteFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtikelZusatzWert_Artikel_ArtikelId1",
                table: "ArtikelZusatzWert");

            migrationBuilder.DropIndex(
                name: "IX_ArtikelZusatzWert_ArtikelId1",
                table: "ArtikelZusatzWert");

            migrationBuilder.DropColumn(
                name: "Mitarbeiter",
                table: "Warenausgaenge");

            migrationBuilder.DropColumn(
                name: "ArtikelId1",
                table: "ArtikelZusatzWert");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zweck",
                table: "Warenausgaenge");

            migrationBuilder.AddColumn<int>(
                name: "Zweck",
                table: "WarenausgangArtikelPositionen",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mitarbeiter",
                table: "Warenausgaenge",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ArtikelId1",
                table: "ArtikelZusatzWert",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelZusatzWert_ArtikelId1",
                table: "ArtikelZusatzWert",
                column: "ArtikelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtikelZusatzWert_Artikel_ArtikelId1",
                table: "ArtikelZusatzWert",
                column: "ArtikelId1",
                principalTable: "Artikel",
                principalColumn: "Id");
        }
    }
}
