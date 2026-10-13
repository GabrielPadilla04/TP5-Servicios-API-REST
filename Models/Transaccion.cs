namespace TP5_Servicios_API_REST.Models
{
    public class Transaccion
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = null!;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Foreign Key: Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // Foreign Key Opcional: Ingresos
        public int? IngresoId { get; set; }
        public Ingreso? Ingreso { get; set; }

        // Foreign Key Opcional: Salidas
        public int? SalidaId { get; set; }
        public Salida? Salida { get; set; }
    }
}