namespace TP5_Servicios_API_REST.DTOs.Usuario.Input
{
    public class UsuarioDtoInput
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}