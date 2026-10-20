namespace PruebaTecnica.Core.Entities;

public class UsuarioMenu
{
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public Guid MenuId { get; set; }
    public Menu Menu { get; set; } = null!;
}
