using TP5_Servicios_API_REST.Data;
using TP5_Servicios_API_REST.Excepciones;
using TP5_Servicios_API_REST.Models;
using Microsoft.EntityFrameworkCore;
using TP5_Servicios_API_REST.DTOs.Producto.Request;
using TP5_Servicios_API_REST.DTOs.Producto.Response; 

namespace TP5_Servicios_API_REST.Services
{
    public class ProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductoListadoResponse>> ObtenerTodos(int page = 1, int pageSize = 10)
        {
            try
            {
                    return await _context.Productos
                    .Include(p => p.Categoria)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new ProductoListadoResponse(
                        p.Id,
                        p.Nombre,
                        p.Precio,
                        p.Stock,
                        p.Categoria != null ? p.Categoria.Nombre : "Sin categoría",
                        p.ImagenUrl))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al obtener los productos: {ex.Message}");
            }
        }

        public async Task<ProductoResponse> ObtenerPorId(int id)
        {
            try
            {
                var producto = await _context.Productos
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (producto == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el producto con ID {id}.");
                }

                return new ProductoResponse(
                    producto.Id,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    producto.CategoriaId,
                    producto.Categoria?.Nombre,
                    producto.ImagenUrl);
            }
            catch (RecursoNoExisteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al obtener el producto: {ex.Message}");
            }
        }

        public async Task<ProductoListadoResponse> Crear(CrearProductoRequest dto)
        {
            try
            {
                // Validar que no exista un producto con el mismo nombre
                bool existe = await _context.Productos.AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower());
                if (existe)
                {
                    throw new RecursoExistenteException($"Ya existe un producto con el nombre '{dto.Nombre}'.");
                }

                // Validar que la categoría asignada realmente exista
                bool categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
                if (!categoriaExiste)
                {
                    throw new RecursoNoExisteException($"La categoría con ID {dto.CategoriaId} no existe.");
                }

                Producto nuevoProducto = new Producto
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Precio = dto.Precio,
                    //el stock inicial es 0 y se aumenta con un Ingreso, pero depende de DTO
                    Stock = dto.Stock,
                    CategoriaId = dto.CategoriaId,
                    ImagenUrl = dto.ImagenUrl // Si la imagen se sube después, esto puede venir nulo
                };

                _context.Productos.Add(nuevoProducto);
                await _context.SaveChangesAsync();

                // Para devolver el nombre de la categoría, la cargamos
                await _context.Entry(nuevoProducto).Reference(p => p.Categoria).LoadAsync();

                return new ProductoListadoResponse(
                    nuevoProducto.Id,
                    nuevoProducto.Nombre,
                    nuevoProducto.Precio,
                    nuevoProducto.Stock,
                    nuevoProducto.Categoria?.Nombre,
                    nuevoProducto.ImagenUrl);
            }
            catch (RecursoExistenteException) { throw; }
            catch (RecursoNoExisteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al crear el producto: {ex.Message}");
            }
        }

        public async Task Actualizar(int id, ActualizarProductoRequest dto)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);

                if (producto == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el producto con ID {id}.");
                }

                // Validar que la nueva categoría exista (si es que la cambió)
                if (producto.CategoriaId != dto.CategoriaId)
                {
                    bool categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
                    if (!categoriaExiste)
                    {
                        throw new RecursoNoExisteException($"La nueva categoría con ID {dto.CategoriaId} no existe.");
                    }
                }

                producto.Nombre = dto.Nombre;
                producto.Descripcion = dto.Descripcion;
                producto.Precio = dto.Precio;
                producto.CategoriaId = dto.CategoriaId;
                // Si la imagen viene en el DTO, la actualizamos
                if (!string.IsNullOrEmpty(dto.ImagenUrl))
                {
                    producto.ImagenUrl = dto.ImagenUrl;
                }

                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al actualizar el producto: {ex.Message}");
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                //Traemos el producto con su historial de compras y ventas
                var producto = await _context.Productos
                    .Include(p => p.IngresoDetalles)
                    .Include(p => p.SalidaDetalles)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (producto == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el producto con ID {id}.");
                }

                //Si el producto ya se compró o se vendió, no se puede borrar
                if (producto.IngresoDetalles != null && producto.IngresoDetalles.Any())
                {
                    throw new DatosLlegaronErradosException($"No se puede eliminar el producto '{producto.Nombre}' porque ya tiene ingresos (compras) registrados.");
                }

                if (producto.SalidaDetalles != null && producto.SalidaDetalles.Any())
                {
                    throw new DatosLlegaronErradosException($"No se puede eliminar el producto '{producto.Nombre}' porque ya tiene salidas (ventas) registradas.");
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (DatosLlegaronErradosException) { throw; } // Atrapamos las validaciones de movimientos
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al eliminar el producto: {ex.Message}");
            }
        }
    }
}