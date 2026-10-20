namespace PruebaTecnica.Core.DTOs;

public class CrearUsuarioDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UsuarioResponseDTO
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
}

public class LoginDTO
{
    public string Correo { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
    public UsuarioResponseDTO Usuario { get; set; } = null!;
}

public class AsignarMenusDTO
{
    public List<Guid> MenuIds { get; set; } = new();
}

public class MenuDTO
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Ruta { get; set; } = string.Empty;
}
