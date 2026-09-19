namespace TP5_Servicios_API_REST.DTOs
{
    public class TransaccionDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; } = null!; // "Entrada" o "Salida"
    }
}