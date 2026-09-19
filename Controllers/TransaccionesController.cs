using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP5_Servicios_API_REST.Data;
using TP5_Servicios_API_REST.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> RegistrarTransaccion(TransaccionDto dto)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst("sub")?.Value;

            if (usuarioIdClaim == null)
                return Unauthorized();

            int usuarioId = int.Parse(usuarioIdClaim);

            // Iniciar transacción explícita
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var producto = await _context.Productos.FindAsync(dto.ProductoId);
                if (producto == null)
                    return NotFound(new { mensaje = "Producto no encontrado." });

                if (dto.Tipo.Equals("Salida", StringComparison.OrdinalIgnoreCase))
                {
                    if (producto.Stock < dto.Cantidad)
                        return BadRequest(new { mensaje = "Stock insuficiente para realizar la salida." });

                    producto.Stock -= dto.Cantidad;
                }
                else if (dto.Tipo.Equals("Entrada", StringComparison.OrdinalIgnoreCase))
                {
                    producto.Stock += dto.Cantidad;
                }
                else
                {
                    return BadRequest(new { mensaje = "Tipo de transacción inválido. Use 'Entrada' o 'Salida'." });
                }

                var transaccion = new Transaccion
                {
                    ProductoId = dto.ProductoId,
                    UsuarioId = usuarioId,
                    Tipo = dto.Tipo,
                    Cantidad = dto.Cantidad,
                    Fecha = DateTime.UtcNow
                };

                _context.Transacciones.Add(transaccion);
                await _context.SaveChangesAsync();

                // Confirmar transacción
                await dbTransaction.CommitAsync();

                return Ok(new { mensaje = "Transacción registrada exitosamente.", nuevoStock = producto.Stock });
            }
            catch (Exception)
            {
                // Revertir ante cualquier error
                await dbTransaction.RollbackAsync();
                return StatusCode(500, "Error interno al procesar la transacción.");
            }
        }
    }
}