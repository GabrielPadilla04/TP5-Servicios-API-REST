using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP5_Servicios_API_REST.Data;
using TP5_Servicios_API_REST.DTOs;
using TP5_Servicios_API_REST.Excepciones;
using TP5_Servicios_API_REST.Models;

namespace TP5_Servicios_API_REST.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransaccionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Transacciones (Obtener la bitácora completa)
        [HttpGet]
        [Authorize(Roles = $"{Roles.Administrador},{Roles.Usuario}")]
        public async Task<ActionResult<IEnumerable<TransaccionReadDto>>> GetTransacciones()
        {
            try
            {
                var transacciones = await _context.Transacciones
                    .AsNoTracking()
                    .OrderByDescending(t => t.Fecha)
                    .Select(t => new TransaccionReadDto
                    {
                        Id = t.Id,
                        Tipo = t.Tipo,
                        Fecha = t.Fecha,
                        UsuarioId = t.UsuarioId,
                        IngresoId = t.IngresoId,
                        SalidaId = t.SalidaId
                    })
                    .ToListAsync();

                return Ok(transacciones);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error al consultar la bitácora",
                    detalle = e.Message
                });
            }
        }

        // POST: api/Transacciones (Registrar transacción en la bitácora)
        [HttpPost]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> RegistrarTransaccion(TransaccionDto dto)
        {
            try
            {
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                     ?? User.FindFirst("sub")?.Value;

                if (usuarioIdClaim == null)
                    return Unauthorized();

                int usuarioId = int.Parse(usuarioIdClaim);

                if (dto.IngresoId == null && dto.SalidaId == null)
                {
                    return BadRequest(new { mensaje = "Debe especificar un IngresoId o un SalidaId." });
                }

                if (!dto.Tipo.Equals("Ingreso", StringComparison.OrdinalIgnoreCase) &&
                    !dto.Tipo.Equals("Salida", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { mensaje = "Tipo de transacción inválido. Use 'Ingreso' o 'Salida'." });
                }

                var transaccion = new Transaccion
                {
                    Tipo = dto.Tipo,
                    Fecha = DateTime.UtcNow,
                    UsuarioId = usuarioId,
                    IngresoId = dto.IngresoId,
                    SalidaId = dto.SalidaId
                };

                _context.Transacciones.Add(transaccion);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Transacción registrada exitosamente en la bitácora.", transaccionId = transaccion.Id });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Error al registrar la transacción",
                    detalle = e.Message
                });
            }
        }
    }
}