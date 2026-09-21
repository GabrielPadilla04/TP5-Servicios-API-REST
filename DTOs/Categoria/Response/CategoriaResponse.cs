namespace TP5_Servicios_API_REST.DTOs.Categoria.Response
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