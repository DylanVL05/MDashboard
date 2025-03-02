using System;
using System.Collections.Generic;

namespace MDashboard.Data.Models;

public partial class Log
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Accion { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
