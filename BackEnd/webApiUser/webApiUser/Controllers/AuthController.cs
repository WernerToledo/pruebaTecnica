using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using webApiUser.AuthenticationService;
using webApiUser.Models;

namespace webApiUser.Controllers
{
    // Define la ruta base para todos los métodos en este controlador
    [Route("/api/v1/auth/login/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        // Inyecta el servicio de autenticación que se usará para manejar el login
        private readonly AuthService _authenticationService;

        // Constructor que recibe una instancia de AuthService a través de la inyección de dependencias
        public AuthController(AuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // Método para manejar la solicitud de login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Auth autenticacion)
        {
            // Verifica si el objeto de autenticación proporcionado es nulo
            if (autenticacion == null)
            {
                // Retorna una respuesta HTTP 400 Bad Request si el objeto es nulo
                return BadRequest("Invalid client request");
            }

            try
            {
                // Llama al servicio de autenticación para validar las credenciales y generar el token
                var authResponse = await _authenticationService.Login(autenticacion);

                // Retorna una respuesta HTTP 200 OK con el resultado de la autenticación, que incluye el token y los detalles del usuario
                return Ok(authResponse);
            }
            catch (AuthenticationException ex)
            {
                // Maneja errores de autenticación, como credenciales incorrectas
                // Retorna una respuesta HTTP 401 Unauthorized con el mensaje de error
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja errores generales que puedan ocurrir
                // Retorna una respuesta HTTP 500 Internal Server Error con un mensaje genérico y detalles del error
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

    }
}
