using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class CategoriaIssue
{
    public int IdCatissue { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<DetalleIssue> DetalleIssues { get; set; } = new List<DetalleIssue>();
}
