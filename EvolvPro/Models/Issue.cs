using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Issue
{
    public int IdIssue { get; set; }

    public string? TituloIssue { get; set; }

    public string? DescripcionIssue { get; set; }

    public DateTime? FechaIssue { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string? ResolucionIssue { get; set; }

    public int? FkEstado { get; set; }

    public int? FkCronograma { get; set; }

    public int? FkRecurso { get; set; }

    public virtual Cronograma? FkCronogramaNavigation { get; set; }

    public virtual DetalleEstado? FkEstadoNavigation { get; set; }

    public virtual Recurso? FkRecursoNavigation { get; set; }
}
