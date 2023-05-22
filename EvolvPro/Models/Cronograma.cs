using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Cronograma
{
    public int IdCronograma { get; set; }

    public string? NombreCrgm { get; set; }

    public string? DescripcionCrgm { get; set; }

    public decimal? HorasCrgm { get; set; }

    public int? Jerarquia { get; set; }

    public int? FkProyecto { get; set; }

    public int? FkEstado { get; set; }

    public int? FkRolhora { get; set; }

    public int? FkRecurso { get; set; }

    public virtual ICollection<DetalleIssue> DetalleIssues { get; set; } = new List<DetalleIssue>();

    public virtual DetalleEstado? FkEstadoNavigation { get; set; }

    public virtual Proyecto? FkProyectoNavigation { get; set; }

    public virtual Recurso? FkRecursoNavigation { get; set; }

    public virtual RolHora? FkRolhoraNavigation { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
