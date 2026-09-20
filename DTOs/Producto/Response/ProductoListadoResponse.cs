namespace TP5_Servicios_API_REST.DTOs.Producto.Response
{
    // DTO Liviano para el listado (ObtenerTodos)
    public class ProductoListadoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? CategoriaNombre { get; set; }
        public string? ImagenUrl { get; set; }

        public ProductoListadoResponse(int id, string nombre, decimal precio, int stock, string? categoriaNombre, string? imagenUrl)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            CategoriaNombre = categoriaNombre;
            ImagenUrl = imagenUrl;
        }
    }
}