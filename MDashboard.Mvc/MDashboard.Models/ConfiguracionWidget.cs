using System;
using System.Collections.Generic;

namespace MDashboard.Models;

public partial class ConfiguracionWidget
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int WidgetId { get; set; }

    public int? Posicion { get; set; }

    public string? Tamano { get; set; }

    public int? RefrescoSegundos { get; set; }

    public bool? EsFavorito { get; set; }

    public bool? EsVisible { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual Widget Widget { get; set; } = null!;
}
