namespace TP5-Servicios-API-REST.DTOs.Categoria.Response
{
    public class CategoriaResponse
    {
       public int Id { get; set; }
       public string Nombre { get; set; }
       public string Descripcion { get; set; }

        public CategoriaResponse(int id, string nombre, string descripcion)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
        }
    }
}