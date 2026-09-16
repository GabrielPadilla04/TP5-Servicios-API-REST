using System.ComponentModel.DataAnnotations;

namespace TP5_Servicios_API_REST.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Rol { get; set; } = "Empleado"; // "Admin" o "Empleado"

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relaciones (1 a N)
        public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
        public ICollection<Salida> Salidas { get; set; } = new List<Salida>();
    }
}