using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class DetalleIssue
{
    public int IdDetalleissue { get; set; }

    public int? FkCategoria { get; set; }

    public int? FkCronograma { get; set; }

    public virtual CategoriaIssue? FkCategoriaNavigation { get; set; }

    public virtual Cronograma? FkCronogramaNavigation { get; set; }
}
