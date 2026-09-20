namespace TP5_Servicios_API_REST.DTOs.Cliente.Response
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Documento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public ClienteResponse(int id, string nombre, string documento, string telefono, string email)
        {
            Id = id;
            Nombre = nombre;
            Documento = documento;
            Telefono = telefono;
            Email = email;
        }
    }
}