using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? NombreCliente { get; set; }

    public string? DireccionCliente { get; set; }

    public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}
