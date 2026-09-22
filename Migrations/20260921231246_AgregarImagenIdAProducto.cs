using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP5_Servicios_API_REST.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenIdAProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImagenId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreOriginal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RutaRelativa = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoContenido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TamanoBytes = table.Column<long>(type: "bigint", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_ImagenId",
                table: "Productos",
                column: "ImagenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Imagenes_ImagenId",
                table: "Productos",
                column: "ImagenId",
                principalTable: "Imagenes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Imagenes_ImagenId",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropIndex(
                name: "IX_Productos_ImagenId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "ImagenId",
                table: "Productos");
        }
    }
}
