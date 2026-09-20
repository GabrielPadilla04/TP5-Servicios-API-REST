namespace TP5-Servicios-API-REST.DTOs.Proveedor.Response
{
    public class ProveedorResponse
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public ProveedorResponse(int id, string razonSocial, string cuit, string telefono, string email)
        {
            Id = id;
            RazonSocial = razonSocial;
            Cuit = cuit;
            Telefono = telefono;
            Email = email;
        }
    }
}