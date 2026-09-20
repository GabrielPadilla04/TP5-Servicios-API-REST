namespace TP5_Servicios_API_REST.DTOs.Producto.Response
{
    // DTO Completo para ver el detalle (ObtenerPorId)
    public class ProductoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public string? ImagenUrl { get; set; }

        public ProductoResponse(int id, string nombre, string descripcion, decimal precio, int stock, int categoriaId, string? categoriaNombre, string? imagenUrl)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            Stock = stock;
            CategoriaId = categoriaId;
            CategoriaNombre = categoriaNombre;
            ImagenUrl = imagenUrl;
        }
    }
}