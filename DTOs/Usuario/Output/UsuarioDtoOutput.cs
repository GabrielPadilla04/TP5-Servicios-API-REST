
namespace TP5_Servicios_API_REST.DTOs.Usuario.Output
{
    // DTO General para listados o detalles
    public class UsuarioDtoOutput
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public DateTime FechaCreacion { get; set; }

        public UsuarioDtoOutput(int id, string nombreCompleto, string email, string rol, DateTime fechaCreacion)
        {
            Id = id;
            NombreCompleto = nombreCompleto;
            Email = email;
            Rol = rol;
            FechaCreacion = fechaCreacion;
        }
    }
}