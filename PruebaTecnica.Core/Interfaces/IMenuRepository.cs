using PruebaTecnica.Core.Entities;

namespace PruebaTecnica.Core.Interfaces;

public interface IMenuRepository
{
    Task<List<Menu>> GetAllAsync();
    Task<Menu?> GetByIdAsync(Guid id);
    Task<Menu> CreateAsync(Menu menu);
}
