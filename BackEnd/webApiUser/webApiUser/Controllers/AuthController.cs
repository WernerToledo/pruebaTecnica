using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using webApiUser.AuthenticationService;
using webApiUser.Models;

namespace webApiUser.Controllers
{
    [Route("/api/v1/auth/login/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _authenticationService;
        public AuthController(AuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Auth autenticacion)
        {
            if (autenticacion == null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                // Llamar al servicio de autenticación para validar las credenciales y generar el token
                var authResponse = await _authenticationService.Login(autenticacion);

                // Retornar la respuesta de autenticación con el token y los detalles del usuario
                return Ok(authResponse);
            }
            catch (AuthenticationException ex)
            {
                // Manejar errores de autenticación, como credenciales incorrectas
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejar errores generales
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

    }
}
