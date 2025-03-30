using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artikelsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTestBenefitsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeBenefits",
                table: "EmployeeBenefits");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EmployeeBenefits",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeBenefit",
                table: "EmployeeBenefits",
                columns: new[] { "EmployeeId", "BenefitId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeBenefit",
                table: "EmployeeBenefits");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EmployeeBenefits",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeBenefits",
                table: "EmployeeBenefits",
                column: "Id");
        }
    }
}
