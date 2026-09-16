using System.ComponentModel.DataAnnotations;

namespace TP5_Servicios_API_REST.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Documento { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Telefono { get; set; }

        [EmailAddress, MaxLength(100)]
        public string? Email { get; set; }

        // Relación con Salidas
        public ICollection<Salida> Salidas { get; set; } = new List<Salida>();
    }
}