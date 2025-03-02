using System;
using System.Collections.Generic;

namespace MDashboard.Data.Models;

public partial class Widget
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int ComponentId { get; set; }

    public string UrlApi { get; set; } = null!;

    public string? ApiKey { get; set; }

    public bool? EsPublico { get; set; }

    public bool? Estado { get; set; }

    public virtual Component Component { get; set; } = null!;

    public virtual ICollection<ConfiguracionWidget> ConfiguracionWidgets { get; set; } = new List<ConfiguracionWidget>();

    public virtual ICollection<PermisosWidget> PermisosWidgets { get; set; } = new List<PermisosWidget>();
}
