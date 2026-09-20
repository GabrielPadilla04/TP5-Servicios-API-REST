namespace TP5-Servicios-API-REST.DTOs.Producto.Request
{
    public class ActualizarProductoRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }
        public string? ImagenUrl { get; set; }
    }
}