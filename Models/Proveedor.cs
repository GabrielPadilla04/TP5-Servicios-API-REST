using System.ComponentModel.DataAnnotations;

namespace TP5_Servicios_API_REST.Models
{
    public class Proveedor
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string RazonSocial { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Cuit { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Telefono { get; set; }

        [EmailAddress, MaxLength(100)]
        public string? Email { get; set; }

        // Relación con Ingresos
        public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
    }
}