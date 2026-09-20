namespace TP5-Servicios-API-REST.DTOs.Usuario.Input
{
    public class UsuarioDtoInput
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Viaja en texto plano y se encripta en el servicio/API
        public string? Rol { get; set; } // Puede ser nulo y el sistema le asigna "Vendedor"
    }
}