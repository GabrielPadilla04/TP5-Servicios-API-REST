namespace TP5_Servicios_API_REST.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public string Tipo { get; set; } = null!; // "Entrada" o "Salida"
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}