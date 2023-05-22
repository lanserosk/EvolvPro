using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class TipoUsuario
{
    public int IdTipousu { get; set; }

    public string? NombreTipo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
