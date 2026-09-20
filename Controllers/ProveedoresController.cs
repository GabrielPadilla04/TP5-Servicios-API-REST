using API.Excepciones;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP5Programacion.Compartidas.DTO.Proveedor.Request;
using TP5Programacion.Compartidas.DTO.Proveedor.Response;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProveedoresController : ControllerBase
    {
        private readonly ProveedorService _proveedorService;

        public ProveedoresController(ProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProveedorResponse>>> ObtenerTodos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Lógica de paginación
                return Ok(await _proveedorService.ObtenerTodos(page, pageSize));
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorResponse>> ObtenerPorId(int id)
        {
            try
            {
                ProveedorResponse proveedor = await _proveedorService.ObtenerPorId(id);
                return Ok(proveedor);
            }
            catch (RecursoNoExisteException e)
            {
                return NotFound(e.Message);
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ProveedorResponse>> Crear(CrearProveedorRequest dto)
        {
            try
            {
                ProveedorResponse proveedor = await _proveedorService.Crear(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = proveedor.Id }, proveedor);
            }
            catch (DatosLlegaronErradosException e)
            {
                return BadRequest(e.Message);
            }
            catch (RecursoExistenteException e) // Ideal para validar que no se repita el CUIT/RUT del proveedor
            {
                return Conflict(e.Message);
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Actualizar(int id, ActualizarProveedorRequest dto)
        {
            try
            {
                await _proveedorService.Actualizar(id, dto);
                return NoContent();
            }
            catch (DatosLlegaronErradosException e)
            {
                return BadRequest(e.Message);
            }
            catch (RecursoNoExisteException e)
            {
                return NotFound(e.Message);
            }
            catch (RecursoExistenteException e)
            {
                return Conflict(e.Message);
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _proveedorService.Eliminar(id);
                return NoContent();
            }
            catch (RecursoNoExisteException e)
            {
                return NotFound(e.Message);
            }
            catch (DatosLlegaronErradosException e)
            {
                return BadRequest(e.Message);
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (Exception e) // Por si intentan borrar un proveedor que ya tiene compras asociadas
            {
                return BadRequest($"No se puede eliminar el proveedor: {e.Message}");
            }
        }
    }
}