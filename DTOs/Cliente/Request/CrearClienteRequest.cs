namespace TP5-Servicios-API-REST.DTOs.Cliente.Request
{
    public class CrearClienteRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}