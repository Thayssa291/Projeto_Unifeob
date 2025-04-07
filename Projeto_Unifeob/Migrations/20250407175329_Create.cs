using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_Unifeob.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beneficio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_beneficio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    percentual = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contribuinte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RazaoSocial = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "date", nullable: false),
                    RegimeTributacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contribuinte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContribuinteBeneficio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContribuinteId = table.Column<int>(type: "int", nullable: false),
                    BeneficioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContribuinteBeneficio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContribuinteBeneficio_Beneficio_BeneficioId",
                        column: x => x.BeneficioId,
                        principalTable: "Beneficio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContribuinteBeneficio_Contribuinte_ContribuinteId",
                        column: x => x.ContribuinteId,
                        principalTable: "Contribuinte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContribuinteBeneficio_BeneficioId",
                table: "ContribuinteBeneficio",
                column: "BeneficioId");

            migrationBuilder.CreateIndex(
                name: "IX_ContribuinteBeneficio_ContribuinteId",
                table: "ContribuinteBeneficio",
                column: "ContribuinteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContribuinteBeneficio");

            migrationBuilder.DropTable(
                name: "Beneficio");

            migrationBuilder.DropTable(
                name: "Contribuinte");
        }
    }
}
