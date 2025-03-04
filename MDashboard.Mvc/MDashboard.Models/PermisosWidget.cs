using System;
using System.Collections.Generic;

namespace MDashboard.Models;

public partial class PermisosWidget
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int WidgetId { get; set; }

    public bool? PuedeVer { get; set; }

    public bool? PuedeEditar { get; set; }

    public bool? PuedeEliminar { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual Widget Widget { get; set; } = null!;
}
