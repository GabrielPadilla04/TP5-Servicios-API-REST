using System.ComponentModel.DataAnnotations.Schema;

namespace TP5_Servicios_API_REST.Models
{
    public class SalidaDetalle
    {
        public int Id { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVenta { get; set; }

        // Claves Foráneas
        public int SalidaId { get; set; }
        public Salida Salida { get; set; } = null!;

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
    }
}