using Microsoft.EntityFrameworkCore;
using TP5_Servicios_API_REST.Data;
using TP5_Servicios_API_REST.Excepciones;
using TP5_Servicios_API_REST.Models;

namespace TP5_Servicios_API_REST.Services
{
    public class ImagenService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ImagenService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<Imagen> SubirImagen(int productoId, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                throw new DatosLlegaronErradosException("No se ha enviado ningún archivo de imagen.");

            var producto = await _context.Productos
                .Include(p => p.Imagen)
                .FirstOrDefaultAsync(p => p.Id == productoId);

            if (producto == null)
                throw new RecursoNoExisteException($"El producto con ID {productoId} no existe.");

            // Determinar la carpeta wwwroot/uploads
            string rootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(rootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Nombre único para evitar sobrescribir imágenes existentes
            string extension = Path.GetExtension(archivo.FileName);
            string nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string rutaFisica = Path.Combine(uploadsFolder, nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            // Actualizar o crear entidad Imagen
            var imagen = producto.Imagen ?? new Imagen();
            imagen.NombreOriginal = archivo.FileName;
            imagen.RutaRelativa = $"uploads/{nombreArchivo}";
            imagen.TipoContenido = archivo.ContentType;
            imagen.TamanoBytes = archivo.Length;
            imagen.FechaCreacion = DateTime.UtcNow;

            if (producto.Imagen == null)
            {
                // Asignamos la navegación directa: EF Core vincula la FK automáticamente al guardar
                producto.Imagen = imagen;
            }

            await _context.SaveChangesAsync();
            return imagen;
        }
    }
}