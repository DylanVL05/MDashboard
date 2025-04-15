using System;
using System.Collections.Generic;

namespace MDashboard.Models;

public partial class ConfiguracionWidget
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int WidgetId { get; set; }

    public int? Height { get; set; }

    public int? Width { get; set; }

    public bool EsFavorito { get; set; }

    public bool EsVisible { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual Widget Widget { get; set; } = null!;
}
