namespace TP5_Servicios_API_REST.DTOs.Proveedor.Request
{
    public class ActualizarProveedorRequest
    {
        public string RazonSocial { get; set; } = string.Empty;
        public string Cuit { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}