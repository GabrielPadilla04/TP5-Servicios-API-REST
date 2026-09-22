namespace TP5_Servicios_API_REST.DTOs.Producto.Response
{
    public record ProductoImagenResponse(
        int Id,
        string NombreOriginal,
        string Url,
        string TipoContenido,
        long TamanoBytes,
        DateTime FechaCreacion
    );
}