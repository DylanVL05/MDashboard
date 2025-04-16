using System;
using System.Collections.Generic;

namespace MDashboard.Models;

public partial class Component
{
    public int Id { get; set; }

    public string Tipo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Widget> Widgets { get; set; } = new List<Widget>();
}
