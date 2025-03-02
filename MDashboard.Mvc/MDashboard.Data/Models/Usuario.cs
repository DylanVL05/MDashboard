using System;
using System.Collections.Generic;

namespace MDashboard.Data.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Rol { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<ConfiguracionWidget> ConfiguracionWidgets { get; set; } = new List<ConfiguracionWidget>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<PermisosWidget> PermisosWidgets { get; set; } = new List<PermisosWidget>();
}
