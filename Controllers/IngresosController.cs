using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5_Servicios_API_REST.Data;
using TP5_Servicios_API_REST.DTOs;
using TP5_Servicios_API_REST.Models;

namespace TP5_Servicios_API_REST.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IngresosController : ControllerBase
{
    private readonly AppDbContext _context;

    public IngresosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarIngreso([FromBody] RegistrarIngresoDto dto)
    {
        if (dto.Detalles == null || !dto.Detalles.Any())
            return BadRequest("Debe incluir al menos un producto en el ingreso.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized("Token no válido.");

            int usuarioId = int.Parse(usuarioIdClaim);

            var ingreso = new Ingreso
            {
                ProveedorId = dto.ProveedorId,
                UsuarioId = usuarioId,
                Fecha = DateTime.UtcNow,
                Total = 0,
                Detalles = new List<IngresoDetalle>()
            };

            decimal totalIngreso = 0;

            foreach (var item in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                {
                    await transaction.RollbackAsync();
                    return NotFound($"El producto con ID {item.ProductoId} no existe.");
                }

                // Incrementamos stock
                producto.Stock += item.Cantidad;

                var subtotal = item.Cantidad * item.PrecioCosto;
                totalIngreso += subtotal;

                // Corregido: Usar .Detalles
                ingreso.Detalles.Add(new IngresoDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioCosto = item.PrecioCosto
                });
            }

            ingreso.Total = totalIngreso;

            _context.Ingresos.Add(ingreso);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { mensaje = "Ingreso registrado y stock incrementado correctamente.", ingresoId = ingreso.Id, total = ingreso.Total });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Error interno al registrar el ingreso: {ex.Message}");
        }
    }
}