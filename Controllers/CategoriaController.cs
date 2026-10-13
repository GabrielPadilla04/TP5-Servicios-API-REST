using TP5_Servicios_API_REST.Excepciones;
using TP5_Servicios_API_REST.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP5_Servicios_API_REST.DTOs.Categoria.Request;
using TP5_Servicios_API_REST.DTOs.Categoria.Response;
using TP5_Servicios_API_REST.Models;

namespace TP5_Servicios_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Administrador},{Roles.Usuario}")]
        public async Task<ActionResult<List<CategoriaResponse>>> ObtenerTodos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _categoriaService.ObtenerTodos(page, pageSize));
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{Roles.Administrador},{Roles.Usuario}")]
        public async Task<ActionResult<CategoriaResponse>> ObtenerPorId(int id)
        {
            try
            {
                CategoriaResponse categoria = await _categoriaService.ObtenerPorId(id);
                return Ok(categoria);
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
        [Authorize(Roles = Roles.Administrador)]
        public async Task<ActionResult<CategoriaResponse>> Crear(CrearCategoriaRequest dto)
        {
            try
            {
                CategoriaResponse categoria = await _categoriaService.Crear(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = categoria.Id }, categoria);
            }
            catch (DatosLlegaronErradosException e)
            {
                return BadRequest(e.Message);
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

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Actualizar(int id, ActualizarCategoriaRequest dto)
        {
            try
            {
                await _categoriaService.Actualizar(id, dto);
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
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _categoriaService.Eliminar(id);
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
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (Exception e)
            {
                return BadRequest($"No se puede eliminar la categoría: {e.Message}");
            }
        }
    }
}