using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Reunione
{
    public int IdReunion { get; set; }

    public string? TituloReu { get; set; }

    public string? PuntosTratar { get; set; }

    public string? DesarrolloPunto { get; set; }

    public string? Asistentes { get; set; }

    public decimal? TiempoReu { get; set; }

    public DateTime? FechaReunion { get; set; }

    public int? FkProyecto { get; set; }

    public virtual Proyecto? FkProyectoNavigation { get; set; }
}
