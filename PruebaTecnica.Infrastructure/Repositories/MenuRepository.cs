using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core.Entities;
using PruebaTecnica.Core.Interfaces;
using PruebaTecnica.Infrastructure.Data;

namespace PruebaTecnica.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly ApplicationDbContext _context;

    public MenuRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Menu>> GetAllAsync()
        => await _context.Menus.Where(m => m.Activo).ToListAsync();

    public async Task<Menu?> GetByIdAsync(Guid id)
        => await _context.Menus.FindAsync(id);

    public async Task<Menu> CreateAsync(Menu menu)
    {
        menu.Id = Guid.NewGuid();
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();
        return menu;
    }
}
