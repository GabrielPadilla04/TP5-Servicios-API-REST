namespace TP5_Servicios_API_REST.Excepciones
{
    public class RecursoNoExisteException : Exception
    {
        public RecursoNoExisteException(string mensaje) : base(mensaje)
        {
        }
    }
}
