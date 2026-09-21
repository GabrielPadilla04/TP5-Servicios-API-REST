namespace TP5_Servicios_API_REST.DTOs.Producto.Request
{
    public class CrearProductoRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public string? ImagenUrl { get; set; } 
    }
}