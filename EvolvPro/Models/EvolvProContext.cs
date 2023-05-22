using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EvolvPro.Models;

public partial class EvolvProContext : DbContext
{
    public EvolvProContext()
    {
    }

    public EvolvProContext(DbContextOptions<EvolvProContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaIssue> CategoriaIssues { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Cronograma> Cronogramas { get; set; }

    public virtual DbSet<DetalleEstado> DetalleEstados { get; set; }

    public virtual DbSet<DetalleIssue> DetalleIssues { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<PreguntaSeguridad> PreguntaSeguridads { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<Recurso> Recursos { get; set; }

    public virtual DbSet<Reunione> Reuniones { get; set; }

    public virtual DbSet<RolHora> RolHoras { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HDAFVPN; Database=EvolvPro; Trusted_Connection=True; Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaIssue>(entity =>
        {
            entity.HasKey(e => e.IdCatissue).HasName("PK__Categori__6C53B46F068F1C85");

            entity.ToTable("CategoriaIssue");

            entity.Property(e => e.IdCatissue).HasColumnName("id_catissue");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__677F38F5ACDA3D79");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.DireccionCliente)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("direccion_cliente");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_cliente");
        });

        modelBuilder.Entity<Cronograma>(entity =>
        {
            entity.HasKey(e => e.IdCronograma).HasName("PK__Cronogra__9C4E436C69CAC108");

            entity.ToTable("Cronograma");

            entity.Property(e => e.IdCronograma).HasColumnName("id_cronograma");
            entity.Property(e => e.DescripcionCrgm)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("descripcion_crgm");
            entity.Property(e => e.FkEstado).HasColumnName("fk_estado");
            entity.Property(e => e.FkProyecto).HasColumnName("fk_proyecto");
            entity.Property(e => e.FkRecurso).HasColumnName("fk_recurso");
            entity.Property(e => e.FkRolhora).HasColumnName("fk_rolhora");
            entity.Property(e => e.HorasCrgm)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("horas_crgm");
            entity.Property(e => e.Jerarquia).HasColumnName("jerarquia");
            entity.Property(e => e.NombreCrgm)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_crgm");

            entity.HasOne(d => d.FkEstadoNavigation).WithMany(p => p.Cronogramas)
                .HasForeignKey(d => d.FkEstado)
                .HasConstraintName("FK__Cronogram__fk_es__403A8C7D");

            entity.HasOne(d => d.FkProyectoNavigation).WithMany(p => p.Cronogramas)
                .HasForeignKey(d => d.FkProyecto)
                .HasConstraintName("FK__Cronogram__fk_pr__3F466844");

            entity.HasOne(d => d.FkRecursoNavigation).WithMany(p => p.Cronogramas)
                .HasForeignKey(d => d.FkRecurso)
                .HasConstraintName("FK__Cronogram__fk_re__4222D4EF");

            entity.HasOne(d => d.FkRolhoraNavigation).WithMany(p => p.Cronogramas)
                .HasForeignKey(d => d.FkRolhora)
                .HasConstraintName("FK__Cronogram__fk_ro__412EB0B6");
        });

        modelBuilder.Entity<DetalleEstado>(entity =>
        {
            entity.HasKey(e => e.IdDetalleestado).HasName("PK__DetalleE__5A4CC0F074660A9F");

            entity.ToTable("DetalleEstado");

            entity.Property(e => e.IdDetalleestado).HasColumnName("id_detalleestado");
            entity.Property(e => e.FkEstado).HasColumnName("fk_estado");
            entity.Property(e => e.ValorDestado)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("valor_destado");

            entity.HasOne(d => d.FkEstadoNavigation).WithMany(p => p.DetalleEstados)
                .HasForeignKey(d => d.FkEstado)
                .HasConstraintName("FK__DetalleEs__fk_es__300424B4");
        });

        modelBuilder.Entity<DetalleIssue>(entity =>
        {
            entity.HasKey(e => e.IdDetalleissue).HasName("PK__DetalleI__0D554F74F6BCCA26");

            entity.ToTable("DetalleIssue");

            entity.Property(e => e.IdDetalleissue).HasColumnName("id_detalleissue");
            entity.Property(e => e.FkCategoria).HasColumnName("fk_categoria");
            entity.Property(e => e.FkCronograma).HasColumnName("fk_cronograma");

            entity.HasOne(d => d.FkCategoriaNavigation).WithMany(p => p.DetalleIssues)
                .HasForeignKey(d => d.FkCategoria)
                .HasConstraintName("FK__DetalleIs__fk_ca__49C3F6B7");

            entity.HasOne(d => d.FkCronogramaNavigation).WithMany(p => p.DetalleIssues)
                .HasForeignKey(d => d.FkCronograma)
                .HasConstraintName("FK__DetalleIs__fk_cr__4AB81AF0");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.IdDocumento).HasName("PK__Document__5D2EE7E57E6FA98F");

            entity.Property(e => e.IdDocumento).HasColumnName("id_documento");
            entity.Property(e => e.Documento1).HasColumnName("documento");
            entity.Property(e => e.FkProyecto).HasColumnName("fk_proyecto");

            entity.HasOne(d => d.FkProyectoNavigation).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.FkProyecto)
                .HasConstraintName("FK__Documento__fk_pr__5070F446");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__86989FB24924210D");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.IdIssue).HasName("PK__Issues__14B38C5C72D55D0A");

            entity.Property(e => e.IdIssue).HasColumnName("id_issue");
            entity.Property(e => e.DescripcionIssue)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion_issue");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("date")
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.FechaIssue)
                .HasColumnType("date")
                .HasColumnName("fecha_issue");
            entity.Property(e => e.FkCronograma).HasColumnName("fk_cronograma");
            entity.Property(e => e.FkEstado).HasColumnName("fk_estado");
            entity.Property(e => e.FkRecurso).HasColumnName("fk_recurso");
            entity.Property(e => e.ResolucionIssue)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("resolucion_issue");
            entity.Property(e => e.TituloIssue)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo_issue");

            entity.HasOne(d => d.FkCronogramaNavigation).WithMany(p => p.Issues)
                .HasForeignKey(d => d.FkCronograma)
                .HasConstraintName("FK__Issues__fk_crono__45F365D3");

            entity.HasOne(d => d.FkEstadoNavigation).WithMany(p => p.Issues)
                .HasForeignKey(d => d.FkEstado)
                .HasConstraintName("FK__Issues__fk_estad__44FF419A");

            entity.HasOne(d => d.FkRecursoNavigation).WithMany(p => p.Issues)
                .HasForeignKey(d => d.FkRecurso)
                .HasConstraintName("FK__Issues__fk_recur__46E78A0C");
        });

        modelBuilder.Entity<PreguntaSeguridad>(entity =>
        {
            entity.HasKey(e => e.IdPregunta).HasName("PK__Pregunta__6867FFA4FE349D01");

            entity.ToTable("PreguntaSeguridad");

            entity.Property(e => e.IdPregunta).HasColumnName("id_pregunta");
            entity.Property(e => e.DescPregunta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("desc_pregunta");
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.IdProyecto).HasName("PK__Proyecto__F38AD81D0F82BAF7");

            entity.Property(e => e.IdProyecto).HasColumnName("id_proyecto");
            entity.Property(e => e.CasoNegocio)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("caso_negocio");
            entity.Property(e => e.FechaFinalProp)
                .HasColumnType("date")
                .HasColumnName("fecha_finalProp");
            entity.Property(e => e.FechaFinalReal)
                .HasColumnType("date")
                .HasColumnName("fecha_finalReal");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.FkCliente).HasColumnName("fk_cliente");
            entity.Property(e => e.FkEstado).HasColumnName("fk_estado");
            entity.Property(e => e.FkUsuario).HasColumnName("fk_usuario");
            entity.Property(e => e.HorasTotales)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("horas_totales");
            entity.Property(e => e.HorasTotalesreal)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("horas_totalesreal");
            entity.Property(e => e.Interesados)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("interesados");
            entity.Property(e => e.NombrePry)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_pry");

            entity.HasOne(d => d.FkClienteNavigation).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.FkCliente)
                .HasConstraintName("FK__Proyectos__fk_cl__37A5467C");

            entity.HasOne(d => d.FkEstadoNavigation).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.FkEstado)
                .HasConstraintName("FK__Proyectos__fk_es__398D8EEE");

            entity.HasOne(d => d.FkUsuarioNavigation).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.FkUsuario)
                .HasConstraintName("FK__Proyectos__fk_us__38996AB5");
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__Recursos__2B386DE4AC85C5E1");

            entity.Property(e => e.IdRecurso).HasColumnName("id_recurso");
            entity.Property(e => e.CorreoRec)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo_rec");
            entity.Property(e => e.NombreRec)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_rec");
            entity.Property(e => e.TelefonoRec)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono_rec");
        });

        modelBuilder.Entity<Reunione>(entity =>
        {
            entity.HasKey(e => e.IdReunion).HasName("PK__Reunione__52FBEC83267167B1");

            entity.Property(e => e.IdReunion).HasColumnName("id_reunion");
            entity.Property(e => e.Asistentes)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("asistentes");
            entity.Property(e => e.DesarrolloPunto)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("desarrollo_punto");
            entity.Property(e => e.FechaReunion)
                .HasColumnType("date")
                .HasColumnName("fecha_reunion");
            entity.Property(e => e.FkProyecto).HasColumnName("fk_proyecto");
            entity.Property(e => e.PuntosTratar)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("puntos_tratar");
            entity.Property(e => e.TiempoReu)
                .HasColumnType("decimal(6, 4)")
                .HasColumnName("tiempo_reu");
            entity.Property(e => e.TituloReu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo_reu");

            entity.HasOne(d => d.FkProyectoNavigation).WithMany(p => p.Reuniones)
                .HasForeignKey(d => d.FkProyecto)
                .HasConstraintName("FK__Reuniones__fk_pr__4D94879B");
        });

        modelBuilder.Entity<RolHora>(entity =>
        {
            entity.HasKey(e => e.IdRolhora).HasName("PK__RolHora__67D9CED00FEE40E5");

            entity.ToTable("RolHora");

            entity.Property(e => e.IdRolhora).HasColumnName("id_rolhora");
            entity.Property(e => e.FkProyecto).HasColumnName("fk_proyecto");
            entity.Property(e => e.HoraTotal)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("hora_total");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
            entity.Property(e => e.ValorHora)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("valor_hora");

            entity.HasOne(d => d.FkProyectoNavigation).WithMany(p => p.RolHoras)
                .HasForeignKey(d => d.FkProyecto)
                .HasConstraintName("FK__RolHora__fk_proy__3C69FB99");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipousu).HasName("PK__TipoUsua__E719BBB25903D77B");

            entity.ToTable("TipoUsuario");

            entity.Property(e => e.IdTipousu).HasColumnName("id_tipousu");
            entity.Property(e => e.NombreTipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__4E3E04AD900ED9FD");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ContrasenaUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasena_usu");
            entity.Property(e => e.CorreoUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo_usu");
            entity.Property(e => e.FkEstadousu).HasColumnName("fk_estadousu");
            entity.Property(e => e.FkPregunta).HasColumnName("fk_pregunta");
            entity.Property(e => e.FkTipousu).HasColumnName("fk_tipousu");
            entity.Property(e => e.NombreUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_usu");
            entity.Property(e => e.RespuestaUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("respuesta_usu");
            entity.Property(e => e.TelefonoUsu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono_usu");

            entity.HasOne(d => d.FkEstadousuNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.FkEstadousu)
                .HasConstraintName("FK__Usuarios__fk_est__32E0915F");

            entity.HasOne(d => d.FkPreguntaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.FkPregunta)
                .HasConstraintName("FK__Usuarios__fk_pre__34C8D9D1");

            entity.HasOne(d => d.FkTipousuNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.FkTipousu)
                .HasConstraintName("FK__Usuarios__fk_tip__33D4B598");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public void SaveChangesAndNotify()
    {
        Subject subject = new Subject();
        base.SaveChanges();
        subject.Notify();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
