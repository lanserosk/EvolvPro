using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<DetalleEstado> DetalleEstados { get; set; } = new List<DetalleEstado>();
}
