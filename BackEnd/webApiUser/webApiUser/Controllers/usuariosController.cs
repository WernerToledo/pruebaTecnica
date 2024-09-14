using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiUser.Models;
using webApiUser.Services;

namespace webApiUser.Controllers
{
    // Define la ruta base para todos los métodos en este controlador
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        // Inyecta el servicio de usuario que se usará para acceder a los datos de usuario
        private readonly usuarioService _usuarioService;

        // Constructor que recibe una instancia de usuarioService a través de la inyección de dependencias
        public usuariosController(usuarioService usuarioRepository)
        {
            _usuarioService = usuarioRepository;
        }

        // Método para obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<usuario>>> GetUsuarios()
        {
            // Llama al servicio para obtener todos los usuarios de manera asíncrona
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            // Retorna una respuesta HTTP 200 OK con la lista de usuarios
            return Ok(usuarios);
        }

        // Método para obtener un usuario por su ID
        [HttpGet("{id}")]
        [Authorize] // Requiere autenticación para acceder a este método
        public async Task<ActionResult<usuario>> GetUsuario(int id)
        {
            // Llama al servicio para obtener un usuario específico por su ID de manera asíncrona
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                // Retorna una respuesta HTTP 404 Not Found si el usuario no existe
                return NotFound();
            }
            // Retorna una respuesta HTTP 200 OK con el usuario encontrado
            return Ok(usuario);
        }

        // Método para crear un nuevo usuario
        [HttpPost]
        [Authorize] // Requiere autenticación para acceder a este método
        public async Task<ActionResult> CreateUsuario([FromBody] usuario usuario)
        {
            // Llama al servicio para agregar un nuevo usuario y obtener su ID
            var id = await _usuarioService.AddUsuarioAsync(usuario);
            // Retorna una respuesta HTTP 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetUsuario), new { id = id }, usuario);
        }

        // Método para actualizar un usuario existente
        [HttpPut("{id}")]
        [Authorize] // Requiere autenticación para acceder a este método
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] usuario usuario)
        {
            // Establece el ID del usuario a actualizar
            usuario.id = id;
            // Llama al servicio para actualizar el usuario
            await _usuarioService.UpdateUsuarioAsync(usuario);
            // Retorna una respuesta HTTP 204 No Content indicando que la actualización fue exitosa
            return NoContent();
        }

        // Método para eliminar un usuario por su ID
        [HttpDelete("{id}")]
        [Authorize] // Requiere autenticación para acceder a este método
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            // Llama al servicio para eliminar el usuario con el ID proporcionado
            await _usuarioService.DeleteUsuarioAsync(id);
            // Retorna una respuesta HTTP 204 No Content indicando que la eliminación fue exitosa
            return NoContent();
        }
    }
}
