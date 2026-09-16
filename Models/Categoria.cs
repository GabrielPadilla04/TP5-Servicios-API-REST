using System.ComponentModel.DataAnnotations;

namespace TP5_Servicios_API_REST.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Descripcion { get; set; }

        // Relación con Productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}