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
public class SalidasController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalidasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarSalida([FromBody] RegistrarSalidaDto dto)
    {
        if (dto.Detalles == null || !dto.Detalles.Any())
            return BadRequest("Debe incluir al menos un producto en la venta.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized("Token no válido.");

            int usuarioId = int.Parse(usuarioIdClaim);

            var salida = new Salida
            {
                ClienteId = dto.ClienteId,
                UsuarioId = usuarioId,
                Fecha = DateTime.UtcNow,
                Total = 0
            };

            decimal totalSalida = 0;

            foreach (var item in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                {
                    await transaction.RollbackAsync();
                    return NotFound($"El producto con ID {item.ProductoId} no existe.");
                }

                // Control estricto de stock suficiente
                if (producto.Stock < item.Cantidad)
                {
                    await transaction.RollbackAsync();
                    return BadRequest($"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, Solicitado: {item.Cantidad}.");
                }

                // Descontar el stock del producto
                producto.Stock -= item.Cantidad;

                var subtotal = item.Cantidad * item.PrecioVenta;
                totalSalida += subtotal;

                salida.SalidaDetalles.Add(new SalidaDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioVenta = item.PrecioVenta
                });
            }

            salida.Total = totalSalida;

            _context.Salidas.Add(salida);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { mensaje = "Venta registrada y stock descontado con éxito.", salidaId = salida.Id, total = salida.Total });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Error interno al registrar la venta: {ex.Message}");
        }
    }
}