using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core.Entities;

namespace PruebaTecnica.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<UsuarioMenu> UsuarioMenus => Set<UsuarioMenu>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar tabla intermedia (muchos a muchos)
        modelBuilder.Entity<UsuarioMenu>()
            .HasKey(um => new { um.UsuarioId, um.MenuId });

        modelBuilder.Entity<UsuarioMenu>()
            .HasOne(um => um.Usuario)
            .WithMany(u => u.UsuarioMenus)
            .HasForeignKey(um => um.UsuarioId);

        modelBuilder.Entity<UsuarioMenu>()
            .HasOne(um => um.Menu)
            .WithMany(m => m.UsuarioMenus)
            .HasForeignKey(um => um.MenuId);

        // Indice unico en correo
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Correo)
            .IsUnique();

        // Datos iniciales - Menus
        modelBuilder.Entity<Menu>().HasData(
            new Menu { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Nombre = "Dashboard", Descripcion = "Panel principal", Ruta = "/dashboard", Activo = true },
            new Menu { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Nombre = "Usuarios", Descripcion = "Administracion de usuarios", Ruta = "/usuarios", Activo = true },
            new Menu { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Nombre = "Reportes", Descripcion = "Modulo de reportes", Ruta = "/reportes", Activo = true },
            new Menu { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Nombre = "Configuracion", Descripcion = "Configuracion del sistema", Ruta = "/configuracion", Activo = true }
        );
    }
}
