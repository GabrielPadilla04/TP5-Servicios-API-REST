namespace TP5_Servicios_API_REST.DTOs.Cliente.Request
{
    public class CrearClienteRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}