using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MDashboard.Models;

namespace MDashboard.Data.Models;

public partial class MediaDashboardContext : DbContext
{
    public MediaDashboardContext()
    {
    }

    public MediaDashboardContext(DbContextOptions<MediaDashboardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ConfiguracionWidget> ConfiguracionWidgets { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<PermisosWidget> PermisosWidgets { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Widget> Widgets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=THELESIS;Database=MediaDashboard;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC07161AFC67");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Tipo).HasMaxLength(50);
        });

        modelBuilder.Entity<ConfiguracionWidget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Configur__3214EC0727A3137D");

            entity.Property(e => e.EsFavorito).HasDefaultValue(false);
            entity.Property(e => e.EsVisible).HasDefaultValue(true);

            entity.HasOne(d => d.Usuario).WithMany(p => p.ConfiguracionWidgets)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Configura__Usuar__4BAC3F29");

            entity.HasOne(d => d.Widget).WithMany(p => p.ConfiguracionWidgets)
                .HasForeignKey(d => d.WidgetId)
                .HasConstraintName("FK__Configura__Widge__4CA06362");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC0728CA5B84");

            entity.Property(e => e.Accion).HasMaxLength(255);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Logs__UsuarioId__4D94879B");
        });

        modelBuilder.Entity<PermisosWidget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permisos__3214EC0702F5FD47");

            entity.Property(e => e.PuedeEditar).HasDefaultValue(false);
            entity.Property(e => e.PuedeEliminar).HasDefaultValue(false);
            entity.Property(e => e.PuedeVer).HasDefaultValue(true);

            entity.HasOne(d => d.Usuario).WithMany(p => p.PermisosWidgets)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__PermisosW__Usuar__4E88ABD4");

            entity.HasOne(d => d.Widget).WithMany(p => p.PermisosWidgets)
                .HasForeignKey(d => d.WidgetId)
                .HasConstraintName("FK__PermisosW__Widge__4F7CD00D");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC0793C31C96");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534484EEB84").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasDefaultValue("Usuario");
        });

        modelBuilder.Entity<Widget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Widgets__3214EC07C5B3FBCF");

            entity.Property(e => e.ApiKey)
                .HasMaxLength(255)
                .HasColumnName("API_Key");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.EsPublico).HasDefaultValue(true);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.UrlApi)
                .HasMaxLength(500)
                .HasColumnName("URL_API");

            entity.HasOne(d => d.Component).WithMany(p => p.Widgets)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK__Widgets__Compone__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
