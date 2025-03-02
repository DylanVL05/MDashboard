using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
        => optionsBuilder.UseSqlServer("Server=DYLANV;Database=MediaDashboard;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC074D49F867");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Tipo).HasMaxLength(50);
        });

        modelBuilder.Entity<ConfiguracionWidget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Configur__3214EC07049AE018");

            entity.Property(e => e.EsFavorito).HasDefaultValue(false);
            entity.Property(e => e.EsVisible).HasDefaultValue(true);
            entity.Property(e => e.Posicion).HasDefaultValue(0);
            entity.Property(e => e.RefrescoSegundos).HasDefaultValue(60);
            entity.Property(e => e.Tamano)
                .HasMaxLength(50)
                .HasDefaultValue("medium");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ConfiguracionWidgets)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Configura__Usuar__47DBAE45");

            entity.HasOne(d => d.Widget).WithMany(p => p.ConfiguracionWidgets)
                .HasForeignKey(d => d.WidgetId)
                .HasConstraintName("FK__Configura__Widge__48CFD27E");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC072D940DAE");

            entity.Property(e => e.Accion).HasMaxLength(255);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Logs__UsuarioId__534D60F1");
        });

        modelBuilder.Entity<PermisosWidget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permisos__3214EC0745475736");

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
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07551825E6");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534925D3512").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Widgets__3214EC076EFC0410");

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
                .HasConstraintName("FK__Widgets__Compone__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
