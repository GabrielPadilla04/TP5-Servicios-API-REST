using API.Excepciones;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP5Programacion.Compartidas.DTO.Cliente.Request;
using TP5Programacion.Compartidas.DTO.Cliente.Response;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteResponse>>> ObtenerTodos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Lógica de paginado
                return Ok(await _clienteService.ObtenerTodos(page, pageSize));
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponse>> ObtenerPorId(int id)
        {
            try
            {
                ClienteResponse cliente = await _clienteService.ObtenerPorId(id);
                return Ok(cliente);
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
        public async Task<ActionResult<ClienteResponse>> Crear(CrearClienteRequest dto)
        {
            try
            {
                ClienteResponse cliente = await _clienteService.Crear(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
            }
            catch (DatosLlegaronErradosException e)
            {
                return BadRequest(e.Message);
            }
            catch (RecursoExistenteException e) //para validar DNI o Correo duplicado
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
        public async Task<IActionResult> Actualizar(int id, ActualizarClienteRequest dto)
        {
            try
            {
                await _clienteService.Actualizar(id, dto);
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
                await _clienteService.Eliminar(id);
                return NoContent();
            }
            catch (RecursoNoExisteException e)
            {
                return NotFound(e.Message);
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (Exception e) // Por si intentan borrar un cliente que ya tiene ventas asociadas
            {
                return BadRequest($"No se puede eliminar el cliente: {e.Message}");
            }
        }
    }
}