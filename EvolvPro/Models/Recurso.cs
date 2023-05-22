using System;
using System.Collections.Generic;

namespace EvolvPro.Models;

public partial class Recurso
{
    public int IdRecurso { get; set; }

    public string? NombreRec { get; set; }

    public string? TelefonoRec { get; set; }

    public string? CorreoRec { get; set; }

    public virtual ICollection<Cronograma> Cronogramas { get; set; } = new List<Cronograma>();

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
