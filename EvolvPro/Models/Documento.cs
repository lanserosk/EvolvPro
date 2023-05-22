using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Documento
{
    public int IdDocumento { get; set; }

    public byte[]? Documento1 { get; set; }

    public int? FkProyecto { get; set; }

    public virtual Proyecto? FkProyectoNavigation { get; set; }
}
