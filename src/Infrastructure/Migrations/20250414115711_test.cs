using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BaseCost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    ZipCode = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventuren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bezeichnung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDatum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AbschlussDatum = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Bemerkung = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventuren", x => x.Id);
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
                    Notizen = table.Column<string>(type: "text", nullable: true),
                    IstAktiv = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lieferanten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produktkategorie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Beschreibung = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produktkategorie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGruppen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGruppen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Nachname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warenausgaenge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AllgemeineBemerkungen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Zweck = table.Column<int>(type: "integer", nullable: false),
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warenausgaenge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wareneingaenge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gesamtpreis = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AllgemeineBemerkungen = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wareneingaenge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zusatzfeld",
                columns: table => new
                {
                    ZusatzfeldID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zusatzfeld", x => x.ZusatzfeldID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeBenefits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    BenefitId = table.Column<int>(type: "integer", nullable: false),
                    CostToEmployee = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBenefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeBenefits_Benefits_BenefitId",
                        column: x => x.BenefitId,
                        principalTable: "Benefits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeBenefits_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
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
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Artikelgruppe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProduktkategorieId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikelgruppe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artikelgruppe_Produktkategorie_ProduktkategorieId",
                        column: x => x.ProduktkategorieId,
                        principalTable: "Produktkategorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGruppenUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    UserGruppenID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGruppenUsers", x => new { x.UserID, x.UserGruppenID });
                    table.ForeignKey(
                        name: "FK_UserGruppenUsers_UserGruppen_UserGruppenID",
                        column: x => x.UserGruppenID,
                        principalTable: "UserGruppen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGruppenUsers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zusatzwert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Wert = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ZusatzFeldID = table.Column<int>(type: "integer", nullable: false),
                    IsChecked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zusatzwert", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zusatzwert_Zusatzfeld_ZusatzFeldID",
                        column: x => x.ZusatzFeldID,
                        principalTable: "Zusatzfeld",
                        principalColumn: "ZusatzfeldID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    HistorischGesetzt = table.Column<bool>(type: "boolean", nullable: false),
                    ArtikelGruppeId = table.Column<int>(type: "integer", nullable: false),
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artikel_Artikelgruppe_ArtikelGruppeId",
                        column: x => x.ArtikelGruppeId,
                        principalTable: "Artikelgruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtikelgruppeZusatzfelder",
                columns: table => new
                {
                    ArtikelgruppeID = table.Column<int>(type: "integer", nullable: false),
                    ZusatzfelderID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikelgruppeZusatzfelder", x => new { x.ArtikelgruppeID, x.ZusatzfelderID });
                    table.ForeignKey(
                        name: "FK_ArtikelgruppeZusatzfelder_Artikelgruppe_ArtikelgruppeID",
                        column: x => x.ArtikelgruppeID,
                        principalTable: "Artikelgruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtikelgruppeZusatzfelder_Zusatzfeld_ZusatzfelderID",
                        column: x => x.ZusatzfelderID,
                        principalTable: "Zusatzfeld",
                        principalColumn: "ZusatzfeldID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "ArtikelZusatzWert",
                columns: table => new
                {
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    ZusatzwertId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikelZusatzWert", x => new { x.ArtikelId, x.ZusatzwertId });
                    table.ForeignKey(
                        name: "FK_ArtikelZusatzWert_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtikelZusatzWert_Zusatzwert_ZusatzwertId",
                        column: x => x.ZusatzwertId,
                        principalTable: "Zusatzwert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventurPositionen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventurId = table.Column<int>(type: "integer", nullable: false),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    GezaehlteMenge = table.Column<int>(type: "integer", nullable: true),
                    IstGeprueft = table.Column<bool>(type: "boolean", nullable: false),
                    DifferenzWert = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Bemerkung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ErstelltVon = table.Column<string>(type: "text", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BearbeitetVon = table.Column<string>(type: "text", nullable: true),
                    BearbeitetAm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "WarenausgangArtikelPositionen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WarenausgangId = table.Column<int>(type: "integer", nullable: false),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Bemerkung = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Artikel_ArtikelGruppeId",
                table: "Artikel",
                column: "ArtikelGruppeId");

            migrationBuilder.CreateIndex(
                name: "IX_Artikelgruppe_ProduktkategorieId",
                table: "Artikelgruppe",
                column: "ProduktkategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelgruppeZusatzfelder_ZusatzfelderID",
                table: "ArtikelgruppeZusatzfelder",
                column: "ZusatzfelderID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelInventurHistorie_ArtikelId",
                table: "ArtikelInventurHistorie",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelInventurHistorie_InventurId",
                table: "ArtikelInventurHistorie",
                column: "InventurId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelLieferanten_ArtikelId",
                table: "ArtikelLieferanten",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelLieferanten_LieferantId",
                table: "ArtikelLieferanten",
                column: "LieferantId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelStatistiken_ArtikelId",
                table: "ArtikelStatistiken",
                column: "ArtikelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtikelZusatzWert_ZusatzwertId",
                table: "ArtikelZusatzWert",
                column: "ZusatzwertId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBenefits_BenefitId",
                table: "EmployeeBenefits",
                column: "BenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBenefits_EmployeeId_BenefitId",
                table: "EmployeeBenefits",
                columns: new[] { "EmployeeId", "BenefitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventurBerichte_InventurId",
                table: "InventurBerichte",
                column: "InventurId");

            migrationBuilder.CreateIndex(
                name: "IX_InventurPositionen_ArtikelId",
                table: "InventurPositionen",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_InventurPositionen_InventurId",
                table: "InventurPositionen",
                column: "InventurId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGruppenUsers_UserGruppenID",
                table: "UserGruppenUsers",
                column: "UserGruppenID");

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

            migrationBuilder.CreateIndex(
                name: "IX_Zusatzwert_ZusatzFeldID",
                table: "Zusatzwert",
                column: "ZusatzFeldID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtikelgruppeZusatzfelder");

            migrationBuilder.DropTable(
                name: "ArtikelInventurHistorie");

            migrationBuilder.DropTable(
                name: "ArtikelLieferanten");

            migrationBuilder.DropTable(
                name: "ArtikelStatistiken");

            migrationBuilder.DropTable(
                name: "ArtikelZusatzWert");

            migrationBuilder.DropTable(
                name: "EmployeeBenefits");

            migrationBuilder.DropTable(
                name: "InventurBerichte");

            migrationBuilder.DropTable(
                name: "InventurPositionen");

            migrationBuilder.DropTable(
                name: "UserGruppenUsers");

            migrationBuilder.DropTable(
                name: "WarenausgangArtikelPositionen");

            migrationBuilder.DropTable(
                name: "WareneingangArtikelPositionen");

            migrationBuilder.DropTable(
                name: "Lieferanten");

            migrationBuilder.DropTable(
                name: "Zusatzwert");

            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Inventuren");

            migrationBuilder.DropTable(
                name: "UserGruppen");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Warenausgaenge");

            migrationBuilder.DropTable(
                name: "Artikel");

            migrationBuilder.DropTable(
                name: "Wareneingaenge");

            migrationBuilder.DropTable(
                name: "Zusatzfeld");

            migrationBuilder.DropTable(
                name: "Artikelgruppe");

            migrationBuilder.DropTable(
                name: "Produktkategorie");
        }
    }
}
