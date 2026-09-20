using TP5-Servicios-API-REST.Data;
using TP5-Servicios-API-REST.Excepciones;
using TP5-Servicios-API-REST.Models;
using Microsoft.EntityFrameworkCore;
using TP5-Servicios-API-REST.DTOs.Cliente.Request;
using TP5-Servicios-API-REST.DTOs.Cliente.Response;

namespace API.Services
{
    public class ClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteResponse>> ObtenerTodos(int page, int pageSize)
        {
            try
            {
                return await _context.Clientes
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new ClienteResponse(c.Id, c.Nombre, c.Documento, c.Telefono, c.Email))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al obtener los clientes: {ex.Message}");
            }
        }

        public async Task<ClienteResponse> ObtenerPorId(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el cliente con ID {id}.");
                }

                return new ClienteResponse(cliente.Id, cliente.Nombre, cliente.Documento, cliente.Telefono, cliente.Email);
            }
            catch (RecursoNoExisteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al obtener el cliente: {ex.Message}");
            }
        }

        public async Task<ClienteResponse> Crear(CrearClienteRequest dto)
        {
            try
            {
                // Validación: No pueden haber dos clientes con el mismo Documento
                bool existeDoc = await _context.Clientes.AnyAsync(c => c.Documento == dto.Documento);
                if (existeDoc)
                {
                    throw new RecursoExistenteException($"Ya existe un cliente registrado con el documento '{dto.Documento}'.");
                }

                Cliente nuevoCliente = new Cliente
                {
                    Nombre = dto.Nombre,
                    Documento = dto.Documento,
                    Telefono = dto.Telefono,
                    Email = dto.Email
                };

                _context.Clientes.Add(nuevoCliente);
                await _context.SaveChangesAsync();

                return new ClienteResponse(nuevoCliente.Id, nuevoCliente.Nombre, nuevoCliente.Documento, nuevoCliente.Telefono, nuevoCliente.Email);
            }
            catch (RecursoExistenteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al crear el cliente: {ex.Message}");
            }
        }

        public async Task Actualizar(int id, ActualizarClienteRequest dto)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el cliente con ID {id}.");
                }

                // Validar si el Documento que mandaron ya lo tiene OTRO cliente distinto
                bool docOcupado = await _context.Clientes
                    .AnyAsync(c => c.Documento == dto.Documento && c.Id != id);

                if (docOcupado)
                {
                    throw new RecursoExistenteException($"El documento '{dto.Documento}' ya pertenece a otro cliente.");
                }

                cliente.Nombre = dto.Nombre;
                cliente.Documento = dto.Documento;
                cliente.Telefono = dto.Telefono;
                cliente.Email = dto.Email;

                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (RecursoExistenteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al actualizar el cliente: {ex.Message}");
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                // Usamos Include para traer el historial de salidas (ventas) de este cliente
                var cliente = await _context.Clientes
                    .Include(c => c.Salidas)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el cliente con ID {id}.");
                }

                // VALIDACIÓN: Si ya le vendimos algo, bloqueamos el borrado
                if (cliente.Salidas != null && cliente.Salidas.Any())
                {
                    throw new DatosLlegaronErradosException($"No se puede eliminar el cliente '{cliente.Nombre}' porque tiene un historial de {cliente.Salidas.Count} salida(s) registrada(s).");
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (DatosLlegaronErradosException) { throw; } // Atrapamos la validación
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al eliminar el cliente: {ex.Message}");
            }
        }
    }
}