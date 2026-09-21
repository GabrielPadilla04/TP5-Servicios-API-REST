using TP5_Servicios_API_REST.DTOs.Usuario.Input;
using TP5_Servicios_API_REST.DTOs.Usuario.Output;
using TP5_Servicios_API_REST.Excepciones;
using TP5_Servicios_API_REST.Services;
using TP5_Servicios_API_REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TP5_Servicios_API_REST.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

      
        // ENDPOINTS DE USUARIO COMÚN
        

        [HttpGet]
        [Authorize(Roles = "Administrador")] 
        public async Task<ActionResult<List<UsuarioDtoOutput>>> ObtenerTodos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var resultado = await _usuarioService.ObtenerlosATodos(page, pageSize);

                // Retornamos el total de registros en los headers para el paginado
                Response.Headers.Append("X-Total-Registros", resultado.TotalRegistros.ToString());

                return Ok(resultado.Usuarios);
            }
            catch (BaseDeDatosException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("dardebaja")]
        public async Task<ActionResult> DarDeBaja()
        {
            try
            {
                string? idClaimUsuario = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (!int.TryParse(idClaimUsuario, out int id))
                {
                    return Unauthorized("No podés dar de baja a este usuario.");
                }

                await _usuarioService.DarDeBaja(id);
                return Ok();
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
        }

        [HttpPut("actualizar")]
        public async Task<ActionResult<UsuarioAuthDtoOutput>> Actualizar([FromBody] UsuarioDtoInput usuarioDtoInput)
        {
            try
            {
                string? idClaimUsuario = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (!int.TryParse(idClaimUsuario, out int id))
                {
                    return Unauthorized("No podés actualizar a este usuario.");
                }

                UsuarioAuthDtoOutput usuarioAuthDtoOutput = await _usuarioService.Actualizar(id, usuarioDtoInput);
                return Ok(usuarioAuthDtoOutput);
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

        
        // ENDPOINTS DE ADMINISTRADOR
        

        [HttpPost("convertirenadministrador/{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ConvertirEnAdministrador(int id)
        {
            try
            {
                await _usuarioService.ConvertirEnAdministrador(id);
                return Ok();
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
        }

        [HttpPost("quitaradministrador/{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> QuitarAdministrador(int id)
        {
            try
            {
                await _usuarioService.QuitarAdministrador(id);
                return Ok();
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
        }

        [HttpDelete("admin/dardebaja/{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DarDeBajaAdmin(int id)
        {
            try
            {
                await _usuarioService.DarDeBaja(id);
                return Ok();
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
        }

        [HttpPut("admin/actualizar/{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ActualizarAdmin(int id, [FromBody] UsuarioDtoInput usuarioDtoInput)
        {
            try
            {
                await _usuarioService.Actualizar(id, usuarioDtoInput);
                return Ok();
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
    }
}