using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnica.Core.DTOs;
using PruebaTecnica.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PruebaTecnica.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IConfiguration _configuration;

    public AuthController(IUsuarioRepository usuarioRepo, IConfiguration configuration)
    {
        _usuarioRepo = usuarioRepo;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO modelo)
    {
        if (string.IsNullOrEmpty(modelo.Correo) || string.IsNullOrEmpty(modelo.Password))
            return BadRequest(new { mensaje = "Correo y password son requeridos" });

        var usuario = await _usuarioRepo.GetByCorreoAsync(modelo.Correo);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(modelo.Password, usuario.PasswordHash))
            return Unauthorized(new { mensaje = "Credenciales invalidas" });

        if (!usuario.Activo)
            return Unauthorized(new { mensaje = "Usuario inactivo" });

        var token = GenerarToken(usuario);

        return Ok(new LoginResponseDTO
        {
            Token = token.Token,
            Expiracion = token.Expiracion,
            Usuario = new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion
            }
        });
    }

    private (string Token, DateTime Expiracion) GenerarToken(PruebaTecnica.Core.Entities.Usuario usuario)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiracion = DateTime.UtcNow.AddMinutes(
            double.Parse(_configuration["Jwt:ExpireMinutes"]!)
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiracion,
            signingCredentials: credenciales
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiracion);
    }
}
