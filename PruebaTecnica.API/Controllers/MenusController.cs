using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Core.DTOs;
using PruebaTecnica.Core.Interfaces;
using System.Security.Claims;

namespace PruebaTecnica.API.Controllers;

[Route("api/menus")]
[ApiController]
[Authorize]
public class MenusController : ControllerBase
{
    private readonly IMenuRepository _menuRepo;
    private readonly IUsuarioRepository _usuarioRepo;

    public MenusController(IMenuRepository menuRepo, IUsuarioRepository usuarioRepo)
    {
        _menuRepo = menuRepo;
        _usuarioRepo = usuarioRepo;
    }

    [HttpGet]
    public async Task<ActionResult<List<MenuDTO>>> GetAll()
    {
        var menus = await _menuRepo.GetAllAsync();
        return Ok(menus.Select(m => new MenuDTO
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Descripcion = m.Descripcion,
            Ruta = m.Ruta
        }).ToList());
    }

    [HttpGet("mis-menus")]
    public async Task<ActionResult<List<MenuDTO>>> GetMisMenus()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var menus = await _usuarioRepo.GetMenusByUsuarioIdAsync(Guid.Parse(userId));
        return Ok(menus.Select(m => new MenuDTO
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Descripcion = m.Descripcion,
            Ruta = m.Ruta
        }).ToList());
    }
}
