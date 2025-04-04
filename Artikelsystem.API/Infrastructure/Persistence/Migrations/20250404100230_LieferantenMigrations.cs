using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artikelsystem.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LieferantenMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IstAktiv",
                table: "Lieferanten",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ArtikelLieferanten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    LieferantId = table.Column<int>(type: "integer", nullable: false),
                    Einkaufspreis = table.Column<decimal>(type: "numeric", nullable: false),
                    Mindestbestellmenge = table.Column<int>(type: "integer", nullable: true),
                    Lieferzeit = table.Column<int>(type: "integer", nullable: true),
                    ArtikelNrBeimLieferanten = table.Column<string>(type: "text", nullable: true),
                    IstAktiv = table.Column<bool>(type: "boolean", nullable: false),
                    IstPrimaerLieferant = table.Column<bool>(type: "boolean", nullable: false),
                    GueltigVon = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GueltigBis = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikelLieferanten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtikelLieferanten_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtikelLieferanten_Lieferanten_LieferantId",
                        column: x => x.LieferantId,
                        principalTable: "Lieferanten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelLieferanten_ArtikelId",
                table: "ArtikelLieferanten",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelLieferanten_LieferantId",
                table: "ArtikelLieferanten",
                column: "LieferantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtikelLieferanten");

            migrationBuilder.DropColumn(
                name: "IstAktiv",
                table: "Lieferanten");
        }
    }
}
