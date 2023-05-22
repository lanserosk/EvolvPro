using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Proyecto
{
    public int IdProyecto { get; set; }

    public string? NombrePry { get; set; }

    public string? CasoNegocio { get; set; }

    public decimal? HorasTotales { get; set; }

    public decimal? HorasTotalesreal { get; set; }

    public string? Interesados { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFinalProp { get; set; }

    public DateTime? FechaFinalReal { get; set; }

    public int? FkCliente { get; set; }

    public int? FkUsuario { get; set; }

    public int? FkEstado { get; set; }

    public virtual ICollection<Cronograma> Cronogramas { get; set; } = new List<Cronograma>();

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public virtual Cliente? FkClienteNavigation { get; set; }

    public virtual DetalleEstado? FkEstadoNavigation { get; set; }

    public virtual Usuario? FkUsuarioNavigation { get; set; }

    public virtual ICollection<Reunione> Reuniones { get; set; } = new List<Reunione>();

    public virtual ICollection<RolHora> RolHoras { get; set; } = new List<RolHora>();
}
