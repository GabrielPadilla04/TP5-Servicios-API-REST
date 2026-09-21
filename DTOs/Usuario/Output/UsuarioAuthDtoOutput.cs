namespace TP5_Servicios_API_REST.DTOs.Usuario.Output
{
    // DTO Específico que se suele devolver al actualizar o al hacer login (sin la fecha de creación)
    public class UsuarioAuthDtoOutput
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }

        public UsuarioAuthDtoOutput(int id, string nombreCompleto, string email, string rol)
        {
            Id = id;
            NombreCompleto = nombreCompleto;
            Email = email;
            Rol = rol;
        }
    }
}