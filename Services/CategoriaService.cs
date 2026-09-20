using TP5_Servicios_API_REST.Data;
using TP5_Servicios_API_REST.Excepciones;
using TP5_Servicios_API_REST.Models;
using Microsoft.EntityFrameworkCore;
using TP5_Servicios_API_REST.DTOs.Categoria.Request;
using TP5_Servicios_API_REST.DTOs.Categoria.Response;

namespace TP5_Servicios_API_REST.Services
{
    public class CategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaResponse>> ObtenerTodos(int page, int pageSize)
    {
        try
        {
            return await _context.Categorias
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoriaResponse(c.Id, c.Nombre, c.Descripcion))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new BaseDeDatosException($"Error al obtener las categorías: {ex.Message}");
        }
    }

    public async Task<CategoriaResponse> ObtenerPorId(int id)
    {
        try
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                throw new RecursoNoExisteException($"No se encontró la categoría con ID {id}.");
            }

            return new CategoriaResponse(categoria.Id, categoria.Nombre, categoria.Descripcion);
        }
        catch (RecursoNoExisteException) { throw; }
        catch (Exception ex)
        {
            throw new BaseDeDatosException($"Error al obtener la categoría: {ex.Message}");
        }
    }

    public async Task<CategoriaResponse> Crear(CrearCategoriaRequest dto)
    {
        try
        {
            bool existe = await _context.Categorias.AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower());
            if (existe)
            {
                throw new RecursoExistenteException($"Ya existe una categoría con el nombre '{dto.Nombre}'.");
            }

            Categoria nuevaCategoria = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            _context.Categorias.Add(nuevaCategoria);
            await _context.SaveChangesAsync();

            return new CategoriaResponse(nuevaCategoria.Id, nuevaCategoria.Nombre, nuevaCategoria.Descripcion);
        }
        catch (RecursoExistenteException) { throw; }
        catch (Exception ex)
        {
            throw new BaseDeDatosException($"Error al crear la categoría: {ex.Message}");
        }
    }

    public async Task Actualizar(int id, ActualizarCategoriaRequest dto)
    {
        try
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                throw new RecursoNoExisteException($"No se encontró la categoría con ID {id}.");
            }

            bool nombreOcupado = await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower() && c.Id != id);

            if (nombreOcupado)
            {
                throw new RecursoExistenteException($"Ya existe otra categoría con el nombre '{dto.Nombre}'.");
            }

            categoria.Nombre = dto.Nombre;
            categoria.Descripcion = dto.Descripcion;

            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }
        catch (RecursoNoExisteException) { throw; }
        catch (RecursoExistenteException) { throw; }
        catch (Exception ex)
        {
            throw new BaseDeDatosException($"Error al actualizar la categoría: {ex.Message}");
        }
    }

    public async Task Eliminar(int id)
    {
        try
        {
            // CAMBIO AQUÍ: Usamos Include para traer la colección de productos y verificar si está vacía
            var categoria = await _context.Categorias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                throw new RecursoNoExisteException($"No se encontró la categoría con ID {id}.");
            }

            // VALIDACIÓN NUEVA: Si la colección tiene al menos 1 producto, bloqueamos el borrado
            if (categoria.Productos.Any())
            {
                throw new DatosLlegaronErradosException($"No se puede eliminar la categoría '{categoria.Nombre}' porque tiene {categoria.Productos.Count} producto(s) asociado(s).");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
        catch (RecursoNoExisteException) { throw; }
        catch (DatosLlegaronErradosException) { throw; } // Atrapamos nuestra nueva validación
        catch (Exception ex)
        {
            throw new BaseDeDatosException($"Error al eliminar la categoría: {ex.Message}");
        }
    }
}
}



