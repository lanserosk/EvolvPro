using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class RolHora
{
    public int IdRolhora { get; set; }

    public string? NombreRol { get; set; }

    public decimal? ValorHora { get; set; }

    public decimal? HoraTotal { get; set; }

    public int? FkProyecto { get; set; }

    public virtual ICollection<Cronograma> Cronogramas { get; set; } = new List<Cronograma>();

    public virtual Proyecto? FkProyectoNavigation { get; set; }
}
