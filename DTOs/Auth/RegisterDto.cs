namespace TP5_Servicios_API_REST.DTOs.Auth
{
    public class RegisterDto
    {
        public string NombreCompleto { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Rol { get; set; } = "Empleado";
    }
}