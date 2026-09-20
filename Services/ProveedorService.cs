using TP5-Servicios - API - REST.Data;
using TP5-Servicios - API - REST.Excepciones;
using TP5-Servicios - API - REST.Models;
using Microsoft.EntityFrameworkCore;
using TP5-Servicios - API - REST.DTOs.Proveedor.Response;
using TP5-Servicios - API - REST.DTOs.Proveedor.Request;

namespace API.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;

        public ProveedorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProveedorResponse>> ObtenerTodos(int page, int pageSize)
        {
            try
            {
                return await _context.Proveedores
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new ProveedorResponse(p.Id, p.RazonSocial, p.Cuit, p.Telefono, p.Email))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al obtener los proveedores: {ex.Message}");
            }
        }

        public async Task<ProveedorResponse> ObtenerPorId(int id)
        {
            try
            {
                var proveedor = await _context.Proveedores.FindAsync(id);

                if (proveedor == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el proveedor con ID {id}.");
                }

                return new ProveedorResponse(proveedor.Id, proveedor.RazonSocial, proveedor.Cuit, proveedor.Telefono, proveedor.Email);
            }
            catch (RecursoNoExisteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al obtener el proveedor: {ex.Message}");
            }
        }

        public async Task<ProveedorResponse> Crear(CrearProveedorRequest dto)
        {
            try
            {
                // Validación de negocio: No pueden haber dos proveedores con el mismo CUIT
                bool existeCuit = await _context.Proveedores.AnyAsync(p => p.Cuit == dto.Cuit);
                if (existeCuit)
                {
                    throw new RecursoExistenteException($"Ya existe un proveedor registrado con el CUIT '{dto.Cuit}'.");
                }

                Proveedor nuevoProveedor = new Proveedor
                {
                    RazonSocial = dto.RazonSocial,
                    Cuit = dto.Cuit,
                    Telefono = dto.Telefono,
                    Email = dto.Email
                };

                _context.Proveedores.Add(nuevoProveedor);
                await _context.SaveChangesAsync();

                return new ProveedorResponse(nuevoProveedor.Id, nuevoProveedor.RazonSocial, nuevoProveedor.Cuit, nuevoProveedor.Telefono, nuevoProveedor.Email);
            }
            catch (RecursoExistenteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al crear el proveedor: {ex.Message}");
            }
        }

        public async Task Actualizar(int id, ActualizarProveedorRequest dto)
        {
            try
            {
                var proveedor = await _context.Proveedores.FindAsync(id);

                if (proveedor == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el proveedor con ID {id}.");
                }

                // Validar si el CUIT que mandaron ya lo tiene OTRO proveedor distinto
                bool cuitOcupado = await _context.Proveedores
                    .AnyAsync(p => p.Cuit == dto.Cuit && p.Id != id);

                if (cuitOcupado)
                {
                    throw new RecursoExistenteException($"El CUIT '{dto.Cuit}' ya pertenece a otro proveedor.");
                }

                proveedor.RazonSocial = dto.RazonSocial;
                proveedor.Cuit = dto.Cuit;
                proveedor.Telefono = dto.Telefono;
                proveedor.Email = dto.Email;

                _context.Proveedores.Update(proveedor);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (RecursoExistenteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al actualizar el proveedor: {ex.Message}");
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                // Usamos Include para traer el historial de ingresos (compras) de este proveedor
                var proveedor = await _context.Proveedores
                    .Include(p => p.Ingresos)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (proveedor == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el proveedor con ID {id}.");
                }

                // VALIDACIÓN: Si ya le compramos algo, no se puede borrar
                if (proveedor.Ingresos != null && proveedor.Ingresos.Any())
                {
                    throw new DatosLlegaronErradosException($"No se puede eliminar el proveedor '{proveedor.RazonSocial}' porque tiene un historial de {proveedor.Ingresos.Count} ingreso(s) registrado(s).");
                }

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (DatosLlegaronErradosException) { throw; } // Atrapamos la validación de los ingresos
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al eliminar el proveedor: {ex.Message}");
            }
        }
    }
}