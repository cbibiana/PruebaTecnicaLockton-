using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Core.DTOs;
using PruebaTecnica.Core.Entities;
using PruebaTecnica.Core.Interfaces;

namespace PruebaTecnica.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepo;

    public UsersController(IUsuarioRepository usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioResponseDTO>> Create([FromBody] CrearUsuarioDTO modelo)
    {
        if (string.IsNullOrEmpty(modelo.Nombre) || string.IsNullOrEmpty(modelo.Correo) || string.IsNullOrEmpty(modelo.Password))
            return BadRequest(new { mensaje = "Todos los campos son requeridos" });

        var existente = await _usuarioRepo.GetByCorreoAsync(modelo.Correo);
        if (existente != null)
            return Conflict(new { mensaje = "Ya existe un usuario con ese correo" });

        var usuario = new Usuario
        {
            Nombre = modelo.Nombre,
            Correo = modelo.Correo,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(modelo.Password)
        };

        var creado = await _usuarioRepo.CreateAsync(usuario);

        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, new UsuarioResponseDTO
        {
            Id = creado.Id,
            Nombre = creado.Nombre,
            Correo = creado.Correo,
            Activo = creado.Activo,
            FechaCreacion = creado.FechaCreacion
        });
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioResponseDTO>>> GetAll()
    {
        var usuarios = await _usuarioRepo.GetAllAsync();
        return Ok(usuarios.Select(u => new UsuarioResponseDTO
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Correo = u.Correo,
            Activo = u.Activo,
            FechaCreacion = u.FechaCreacion
        }).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioResponseDTO>> GetById(Guid id)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado" });

        return Ok(new UsuarioResponseDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Activo = usuario.Activo,
            FechaCreacion = usuario.FechaCreacion
        });
    }

    [HttpGet("{id}/menus")]
    public async Task<ActionResult<List<MenuDTO>>> GetMenus(Guid id)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado" });

        var menus = await _usuarioRepo.GetMenusByUsuarioIdAsync(id);
        return Ok(menus.Select(m => new MenuDTO
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Descripcion = m.Descripcion,
            Ruta = m.Ruta
        }).ToList());
    }

    [HttpPut("{id}/menus")]
    public async Task<ActionResult> AsignarMenus(Guid id, [FromBody] AsignarMenusDTO modelo)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado" });

        await _usuarioRepo.AsignarMenusAsync(id, modelo.MenuIds);
        return Ok(new { mensaje = "Menus asignados correctamente" });
    }
}
