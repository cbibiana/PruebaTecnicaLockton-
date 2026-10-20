using PruebaTecnica.Core.Entities;

namespace PruebaTecnica.Core.Interfaces;

public interface IUsuarioRepository
{
    Task<List<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(Guid id);
    Task<Usuario?> GetByCorreoAsync(string correo);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<List<Menu>> GetMenusByUsuarioIdAsync(Guid usuarioId);
    Task AsignarMenusAsync(Guid usuarioId, List<Guid> menuIds);
}
