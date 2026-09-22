namespace TP5_Servicios_API_REST.Models
{
    public class Imagen
    {
        public int Id { get; set; }
        public string NombreOriginal { get; set; } = string.Empty;
        public string RutaRelativa { get; set; } = string.Empty;
        public string TipoContenido { get; set; } = string.Empty;
        public long TamanoBytes { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}