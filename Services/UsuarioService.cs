using TP5-Servicios - API - REST.Data;
using TP5-Servicios - API - REST.Excepciones;
using TP5-Servicios - API - REST.Models;
using Microsoft.EntityFrameworkCore;
using TP5-Servicios - API - REST.DTOs.Input;
using TP5-Servicios - API - REST.DTOs.Output;

namespace TP5-Servicios - API - REST.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioDtoOutput>> ObtenerlosATodos(int page = 1, int pageSize = 10)
        {
            try
            {
                return await _context.Usuarios
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UsuarioDtoOutput(
                        u.Id,
                        u.NombreCompleto,
                        u.Email,
                        u.Rol,
                        u.FechaCreacion))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al obtener los usuarios: {ex.Message}");
            }
        }

        // Si necesitas crear usuarios desde un registro público o panel admin
        public async Task<UsuarioDtoOutput> Crear(UsuarioDtoInput dto)
        {
            try
            {
                bool existeEmail = await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower());
                if (existeEmail)
                {
                    throw new RecursoExistenteException($"Ya existe un usuario con el email '{dto.Email}'.");
                }

                Usuario nuevoUsuario = new Usuario
                {
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    // IMPORTANTE: Aquí deberías usar BCrypt para encriptar, por ahora lo pasamos directo
                    PasswordHash = dto.Password,
                    Rol = dto.Rol ?? "Vendedor", // Por defecto le asignamos Vendedor
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                return new UsuarioDtoOutput(nuevoUsuario.Id, nuevoUsuario.NombreCompleto, nuevoUsuario.Email, nuevoUsuario.Rol, nuevoUsuario.FechaCreacion);
            }
            catch (RecursoExistenteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al crear el usuario: {ex.Message}");
            }
        }

        public async Task<UsuarioAuthDtoOutput> Actualizar(int id, UsuarioDtoInput dto)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el usuario con ID {id}.");
                }

                bool emailOcupado = await _context.Usuarios
                    .AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.Id != id);

                if (emailOcupado)
                {
                    throw new RecursoExistenteException($"El email '{dto.Email}' ya pertenece a otro usuario.");
                }

                usuario.NombreCompleto = dto.NombreCompleto;
                usuario.Email = dto.Email;

                // Solo actualizamos la contraseña si mandó una nueva
                if (!string.IsNullOrEmpty(dto.Password))
                {
                    usuario.PasswordHash = dto.Password;
                }

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                // Adaptá esto según lo que requiera el constructor de tu UsuarioAuthDtoOutput
                return new UsuarioAuthDtoOutput(usuario.Id, usuario.NombreCompleto, usuario.Email, usuario.Rol);
            }
            catch (RecursoNoExisteException) { throw; }
            catch (RecursoExistenteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al actualizar el usuario: {ex.Message}");
            }
        }

        public async Task DarDeBaja(int id)
        {
            try
            {
                //Revisamos si el usuario procesó ventas o compras
                var usuario = await _context.Usuarios
                    .Include(u => u.Ingresos)
                    .Include(u => u.Salidas)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    throw new RecursoNoExisteException($"No se encontró el usuario con ID {id}.");
                }

                // Si ya operó en el sistema, no podemos borrarlo físicamente
                if ((usuario.Ingresos != null && usuario.Ingresos.Any()) ||
                    (usuario.Salidas != null && usuario.Salidas.Any()))
                {
                    throw new DatosLlegaronErradosException($"No se puede dar de baja al usuario '{usuario.NombreCompleto}' porque tiene un historial de operaciones (compras o ventas) registradas en el sistema.");

                }
                else
                {
                    // Si nunca operó, lo borramos de verdad
                    _context.Usuarios.Remove(usuario);
                }

                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (DatosLlegaronErradosException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al dar de baja el usuario: {ex.Message}");
            }
        }

        public async Task ConvertirEnAdministrador(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) throw new RecursoNoExisteException($"No se encontró el usuario con ID {id}.");

                usuario.Rol = "Administrador";
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al cambiar el rol a Administrador: {ex.Message}");
            }
        }

        public async Task QuitarAdministrador(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) throw new RecursoNoExisteException($"No se encontró el usuario con ID {id}.");

                usuario.Rol = "Vendedor"; // O el rol base que utilicen
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (RecursoNoExisteException) { throw; }
            catch (Exception ex)
            {
                throw new BaseDeDatosException($"Error al quitar el rol de Administrador: {ex.Message}");
            }
        }
    }
}