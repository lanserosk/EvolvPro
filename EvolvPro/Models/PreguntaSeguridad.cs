using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class PreguntaSeguridad
{
    public int IdPregunta { get; set; }

    public string? DescPregunta { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
