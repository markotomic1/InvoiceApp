using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialMIgration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fakture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Broj = table.Column<int>(type: "integer", nullable: false),
                    Datum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Partner = table.Column<string>(type: "text", nullable: true),
                    IznosBezPdv = table.Column<float>(type: "real", nullable: false),
                    PostoRabata = table.Column<float>(type: "real", nullable: false),
                    Rabat = table.Column<float>(type: "real", nullable: false),
                    IznosSaRabatomBezPdv = table.Column<float>(type: "real", nullable: false),
                    Pdv = table.Column<float>(type: "real", nullable: false),
                    Ukupno = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fakture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StavkeFakture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rbr = table.Column<int>(type: "integer", nullable: false),
                    NazivArtikla = table.Column<string>(type: "text", nullable: true),
                    Kolicina = table.Column<int>(type: "integer", nullable: false),
                    Cijena = table.Column<float>(type: "real", nullable: false),
                    IznosBezPdv = table.Column<float>(type: "real", nullable: false),
                    PostoRabata = table.Column<float>(type: "real", nullable: false),
                    Rabat = table.Column<float>(type: "real", nullable: false),
                    IznosSaRabatomBezPdv = table.Column<float>(type: "real", nullable: false),
                    Pdv = table.Column<float>(type: "real", nullable: false),
                    Ukupno = table.Column<float>(type: "real", nullable: false),
                    FakturaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StavkeFakture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StavkeFakture_Fakture_FakturaId",
                        column: x => x.FakturaId,
                        principalTable: "Fakture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fakture_Broj",
                table: "Fakture",
                column: "Broj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StavkeFakture_FakturaId",
                table: "StavkeFakture",
                column: "FakturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StavkeFakture");

            migrationBuilder.DropTable(
                name: "Fakture");
        }
    }
}
