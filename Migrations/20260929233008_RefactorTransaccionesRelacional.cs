using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP5_Servicios_API_REST.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTransaccionesRelacional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Productos_ProductoId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_ProductoId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "ProductoId",
                table: "Transacciones");

            migrationBuilder.AddColumn<int>(
                name: "IngresoId",
                table: "Transacciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalidaId",
                table: "Transacciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_IngresoId",
                table: "Transacciones",
                column: "IngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_SalidaId",
                table: "Transacciones",
                column: "SalidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Ingresos_IngresoId",
                table: "Transacciones",
                column: "IngresoId",
                principalTable: "Ingresos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Salidas_SalidaId",
                table: "Transacciones",
                column: "SalidaId",
                principalTable: "Salidas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Ingresos_IngresoId",
                table: "Transacciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Salidas_SalidaId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_IngresoId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_SalidaId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "IngresoId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "SalidaId",
                table: "Transacciones");

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Transacciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductoId",
                table: "Transacciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ProductoId",
                table: "Transacciones",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Productos_ProductoId",
                table: "Transacciones",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
