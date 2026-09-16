using System.ComponentModel.DataAnnotations.Schema;

namespace TP5_Servicios_API_REST.Models
{
    public class Ingreso
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // Claves Foráneas
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; } = null!;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // Detalle de compra
        public ICollection<IngresoDetalle> Detalles { get; set; } = new List<IngresoDetalle>();
    }
}