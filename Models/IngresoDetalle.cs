using System.ComponentModel.DataAnnotations.Schema;

namespace TP5_Servicios_API_REST.Models
{
    public class IngresoDetalle
    {
        public int Id { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCosto { get; set; }

        // Claves Foráneas
        public int IngresoId { get; set; }
        public Ingreso Ingreso { get; set; } = null!;

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
    }
}