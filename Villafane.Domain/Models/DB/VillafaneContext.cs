using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Villafane.Domain.Models.DB
{
    public partial class VillafaneContext : DbContext
    {
        public VillafaneContext()
        {
        }

        public VillafaneContext(DbContextOptions<VillafaneContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Configuracion> Configuracions { get; set; } = null!;
        public virtual DbSet<Escala> Escalas { get; set; } = null!;
        public virtual DbSet<GruposDeIntere> GruposDeInteres { get; set; } = null!;
        public virtual DbSet<Indicadore> Indicadores { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<LogotipoCliente> LogotipoClientes { get; set; } = null!;
        public virtual DbSet<Periodo> Periodos { get; set; } = null!;
        public virtual DbSet<PesosValoresDefinitivo> PesosValoresDefinitivos { get; set; } = null!;
        public virtual DbSet<PesosValoresTeorico> PesosValoresTeoricos { get; set; } = null!;
        public virtual DbSet<PesosVariablesTeorico> PesosVariablesTeoricos { get; set; } = null!;
        public virtual DbSet<Valore> Valores { get; set; } = null!;
        public virtual DbSet<Variable> Variables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=srvsqlvillafane.database.windows.net;Database=dbazsqlcltcetelem;Trusted_Connection=False;user id=adminvillafane;password=IozhHBZiNm0Z4BZmctUi;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.ToTable("Configuracion");

                entity.Property(e => e.ColorBarra)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ColorCabeceraTablasVertical)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ColorCabecerasTablas)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ColorPrincipal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdLogoNavigation)
                    .WithMany(p => p.Configuracions)
                    .HasForeignKey(d => d.IdLogo)
                    .HasConstraintName("FK_Configuracion_Logotipo_Cliente");
            });

            modelBuilder.Entity<Escala>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Max).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Min).HasColumnType("decimal(18, 5)");
            });

            modelBuilder.Entity<GruposDeIntere>(entity =>
            {
                entity.Property(e => e.Grupo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PesoEnIr)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("PesoEnIR");
            });

            modelBuilder.Entity<Indicadore>(entity =>
            {
                entity.Property(e => e.Dato).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.DatoNormalizado).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Fuente)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Indicador)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NotaDeLaVariable).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.NotaDelValor).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Periodo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PesoDelGdienValor)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("PesoDelGDIEnValor");

                entity.Property(e => e.PesoIndicadorEnGdi)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("PesoIndicadorEnGDI");

                entity.Property(e => e.PesoIndicadorEnGdinormalizado)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("PesoIndicadorEnGDINormalizado");

                entity.Property(e => e.PesoIndicadorEnIr)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("PesoIndicadorEnIR");

                entity.Property(e => e.PesoIndicadorEnValor).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.PesoIndicadorEnValorNormalizado).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.PesoIndicadorEnVariable).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.PesoIndicadorEnVariableNormalizado).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.IdEscalaNavigation)
                    .WithMany(p => p.Indicadores)
                    .HasForeignKey(d => d.IdEscala)
                    .HasConstraintName("FK_Indicadores_Escalas");

                entity.HasOne(d => d.IdGrupoInteresNavigation)
                    .WithMany(p => p.Indicadores)
                    .HasForeignKey(d => d.IdGrupoInteres)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Indicadores_GruposDeInteres");

                entity.HasOne(d => d.IdVariableNavigation)
                    .WithMany(p => p.Indicadores)
                    .HasForeignKey(d => d.IdVariable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Indicadores_Variables");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Accion).IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Pantalla)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogotipoCliente>(entity =>
            {
                entity.ToTable("Logotipo_Cliente");

                entity.Property(e => e.ContentTypeLogo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionLogo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Periodo>(entity =>
            {
                entity.Property(e => e.PeriodoDeEjecucion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PesosValoresDefinitivo>(entity =>
            {
                entity.ToTable("PesosValoresDefinitivo");

                entity.Property(e => e.ValorDefinitivo).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.IdGrupoInteresNavigation)
                    .WithMany(p => p.PesosValoresDefinitivos)
                    .HasForeignKey(d => d.IdGrupoInteres)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PesosValoresDefinitivo_GruposDeInteres");

                entity.HasOne(d => d.IdVariableNavigation)
                    .WithMany(p => p.PesosValoresDefinitivos)
                    .HasForeignKey(d => d.IdVariable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PesosValoresDefinitivo_Variables");
            });

            modelBuilder.Entity<PesosValoresTeorico>(entity =>
            {
                entity.Property(e => e.InformacionDisponible)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Periodo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tb12)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB12");

                entity.Property(e => e.Tb21)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB21");

                entity.Property(e => e.Tb22)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB22");

                entity.Property(e => e.Tb23)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB23");

                entity.Property(e => e.Tb3)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB3");

                entity.Property(e => e.Tb4)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB4");

                entity.Property(e => e.ValorTeorico).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.IdGrupoInteresNavigation)
                    .WithMany(p => p.PesosValoresTeoricos)
                    .HasForeignKey(d => d.IdGrupoInteres)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PesosValoresTeoricos_GruposDeInteres");

                entity.HasOne(d => d.IdValorNavigation)
                    .WithMany(p => p.PesosValoresTeoricos)
                    .HasForeignKey(d => d.IdValor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PesosValoresTeoricos_Valores");
            });

            modelBuilder.Entity<PesosVariablesTeorico>(entity =>
            {
                entity.ToTable("PesosVariablesTeorico");

                entity.Property(e => e.Periodo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tb12)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB12");

                entity.Property(e => e.Tb21)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB21");

                entity.Property(e => e.Tb22)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB22");

                entity.Property(e => e.Tb23)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB23");

                entity.Property(e => e.Tb3)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB3");

                entity.Property(e => e.Tb4)
                    .HasColumnType("decimal(18, 5)")
                    .HasColumnName("TB4");

                entity.Property(e => e.ValorTeorico).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.IdGrupoInteresNavigation)
                    .WithMany(p => p.PesosVariablesTeoricos)
                    .HasForeignKey(d => d.IdGrupoInteres)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PesosVariablesTeorico_GruposDeInteres");

                entity.HasOne(d => d.IdVariableNavigation)
                    .WithMany(p => p.PesosVariablesTeoricos)
                    .HasForeignKey(d => d.IdVariable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PesosVariablesTeorico_Variables");
            });

            modelBuilder.Entity<Valore>(entity =>
            {
                entity.Property(e => e.Valor).IsUnicode(false);
            });

            modelBuilder.Entity<Variable>(entity =>
            {
                entity.Property(e => e.Variable1)
                    .IsUnicode(false)
                    .HasColumnName("Variable");

                entity.HasOne(d => d.IdValorNavigation)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.IdValor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Variables_Valores");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
