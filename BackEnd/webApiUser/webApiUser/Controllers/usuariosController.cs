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
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly usuarioService _usuarioService;

        public usuariosController(usuarioService usuarioRepository)
        {
            _usuarioService = usuarioRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateUsuario([FromBody] usuario usuario)
        {
            var id = await _usuarioService.AddUsuarioAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = id }, usuario);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] usuario usuario)
        {
            usuario.id = id;
            await _usuarioService.UpdateUsuarioAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            await _usuarioService.DeleteUsuarioAsync(id);
            return NoContent();
        }
    }
}
