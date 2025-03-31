using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artikelsystem.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DbContextAusgegliedert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WareneingangArtikel_WareneingangId",
                table: "WareneingangArtikel");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikel_WareneingangId_ArtikelId",
                table: "WareneingangArtikel",
                columns: new[] { "WareneingangId", "ArtikelId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WareneingangArtikel_WareneingangId_ArtikelId",
                table: "WareneingangArtikel");

            migrationBuilder.CreateIndex(
                name: "IX_WareneingangArtikel_WareneingangId",
                table: "WareneingangArtikel",
                column: "WareneingangId");
        }
    }
}
