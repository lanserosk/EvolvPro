using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsu { get; set; }

    public string? TelefonoUsu { get; set; }

    public string? CorreoUsu { get; set; }

    public string? ContrasenaUsu { get; set; }

    public string? RespuestaUsu { get; set; }

    public int? FkEstadousu { get; set; }

    public int? FkTipousu { get; set; }

    public int? FkPregunta { get; set; }

    public virtual DetalleEstado? FkEstadousuNavigation { get; set; }

    public virtual PreguntaSeguridad? FkPreguntaNavigation { get; set; }

    public virtual TipoUsuario? FkTipousuNavigation { get; set; }

    public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}
