using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class DetalleEstado
{
    public int IdDetalleestado { get; set; }

    public string? ValorDestado { get; set; }

    public int? FkEstado { get; set; }

    public virtual ICollection<Cronograma> Cronogramas { get; set; } = new List<Cronograma>();

    public virtual Estado? FkEstadoNavigation { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
