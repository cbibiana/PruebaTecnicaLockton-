using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core.Entities;
using PruebaTecnica.Core.Interfaces;
using PruebaTecnica.Infrastructure.Data;

namespace PruebaTecnica.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllAsync()
        => await _context.Usuarios.ToListAsync();

    public async Task<Usuario?> GetByIdAsync(Guid id)
        => await _context.Usuarios.FindAsync(id);

    public async Task<Usuario?> GetByCorreoAsync(string correo)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        usuario.Id = Guid.NewGuid();
        usuario.FechaCreacion = DateTime.UtcNow;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<List<Menu>> GetMenusByUsuarioIdAsync(Guid usuarioId)
    {
        return await _context.UsuarioMenus
            .Where(um => um.UsuarioId == usuarioId)
            .Include(um => um.Menu)
            .Select(um => um.Menu)
            .Where(m => m.Activo)
            .ToListAsync();
    }

    public async Task AsignarMenusAsync(Guid usuarioId, List<Guid> menuIds)
    {
        // Eliminar asignaciones actuales
        var actuales = await _context.UsuarioMenus
            .Where(um => um.UsuarioId == usuarioId)
            .ToListAsync();
        _context.UsuarioMenus.RemoveRange(actuales);

        // Agregar nuevas asignaciones
        foreach (var menuId in menuIds)
        {
            _context.UsuarioMenus.Add(new UsuarioMenu
            {
                UsuarioId = usuarioId,
                MenuId = menuId
            });
        }

        await _context.SaveChangesAsync();
    }
}
