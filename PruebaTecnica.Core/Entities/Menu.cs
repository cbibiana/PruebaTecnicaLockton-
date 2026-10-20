namespace PruebaTecnica.Core.Entities;

public class Menu
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Ruta { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    // Navegacion
    public ICollection<UsuarioMenu> UsuarioMenus { get; set; } = new List<UsuarioMenu>();
}
