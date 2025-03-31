using Avaluo.Infrastructure.Data.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data
{
    public class AvaluoDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AvaluoDbContext(IConfiguration configuration, DbContextOptions<AvaluoDbContext> options)
            : base(options)
        {
            _configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public DbSet<Estado> Estados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rubrica> Rubricas { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<ProfesorCarrera> ProfesorCarreras { get; set; }
        public DbSet<CarreraRubrica> CarreraRubricas { get; set; }
        public DbSet<ConfiguracionEvaluaciones> Configuraciones { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<MetodoEvaluacion> MetodosEvaluacion { get; set; }
        public DbSet<TipoCompetencia> TiposCompetencia { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<MapaCompetencias> MapaCompetencias { get; set; }
        public DbSet<TipoInforme> TiposInforme { get; set; }
        public DbSet<Informe> Informes { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Edificio> Edificios { get; set; }
        public DbSet<ObjetoAula> ObjetosAula { get; set; }
        public DbSet<AsignaturaCarrera> AsignaturasPensum { get; set; }
        public DbSet<HistorialIncumplimiento> HistorialIncumplimientos { get; set; }
        public DbSet<Evidencia> Evidencias { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Desempeno> Desempenos { get; set; }
        public DbSet<SOEvaluacion> SOEvaluaciones { get; set; }
        public DbSet<ActionPlan> ActionPlans { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<PI> PIs { get; set; }
        public DbSet<Resumen> Resumenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("UsuarioSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("RubricaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("AsignaturaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("AreaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("CarreraSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("RolSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("PISequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("InformeSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("EdificioSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("DesempenoSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("ContactoSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("EvidenciaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("AulaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("InventarioSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("TareaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("TipoCompetenciaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("TipoInformeSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("ActionPlanSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("ConfiguracionSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("CompetenciaSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("HistorialIncumplimientoSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("MetodoEvaluacionSequence").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("EstadoSequence").StartsAt(1).IncrementsBy(1);

            // Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Usuario");

                entity.ToTable("usuario");

                entity.HasIndex(e => e.IdSO, "Id_SO");
                entity.HasIndex(e => e.IdArea, "Id_Area");
                entity.HasIndex(e => e.IdRol, "Id_Rol");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");

                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR UsuarioSequence");
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.HashedPassword).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UltimaEdicion).IsRequired(false);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Foto).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CV).HasMaxLength(int.MaxValue);

                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usuario_ibfk_1");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("usuario_ibfk_2");

                entity.HasOne(d => d.SO)
                    .WithMany(p => p.Profesores)
                    .HasForeignKey(d => d.IdSO)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("usuario_ibfk_3");

                entity.HasMany(d => d.Contactos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("usuario_ibfk_4");

                entity.HasMany(d => d.HistorialIncumplimientos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("usuario_ibfk_5");

                entity.HasMany(d => d.Rubricas)
                    .WithOne(p => p.Profesor)
                    .HasForeignKey(p => p.IdProfesor)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("usuario_ibfk_6");

                entity.HasMany(d => d.ProfesoresCarreras)
                    .WithOne(p => p.Profesor)
                    .HasForeignKey(p => p.IdProfesor)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("usuario_ibfk_7");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("usuarios_ibfk_8");

            });

            // Rubrica
            modelBuilder.Entity<Rubrica>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Rubrica");
                entity.ToTable("rubricas");
                entity.HasIndex(e => e.IdSO, "Id_SO");
                entity.HasIndex(e => e.IdProfesor, "Id_Profesor");
                entity.HasIndex(e => e.IdAsignatura, "Id_Asignatura");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");
                entity.HasIndex(e => e.IdMetodoEvaluacion, "Id_MetodoEvaluacion");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR RubricaSequence");
                entity.Property(e => e.FechaCompletado).IsRequired(false);
                entity.Property(e => e.UltimaEdicion).IsRequired(false);
                entity.Property(e => e.Periodo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Seccion).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Evidencia).IsRequired(false).HasMaxLength(int.MaxValue);
                entity.Property(e => e.Comentario).IsRequired(false).HasMaxLength(int.MaxValue);
                entity.Property(e => e.Problematica).IsRequired(false).HasMaxLength(int.MaxValue);
                entity.Property(e => e.Solucion).IsRequired(false).HasMaxLength(int.MaxValue);
                entity.Property(e => e.EvaluacionesFormativas).IsRequired(false).HasMaxLength(int.MaxValue);
                entity.Property(e => e.Estrategias).IsRequired(false).HasMaxLength(int.MaxValue);
                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Rubricas)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("rubricas_estado_fk");
                entity.HasOne(d => d.MetodoEvaluacion)
                    .WithMany(p => p.Rubricas)
                    .HasForeignKey(d => d.IdMetodoEvaluacion)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("rubricas_metodo_fk");
                entity.HasOne(d => d.Profesor)
                    .WithMany(p => p.Rubricas)
                    .HasForeignKey(d => d.IdProfesor)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("rubricas_profesor_fk");
                entity.HasOne(d => d.Asignatura)
                    .WithMany(p => p.Rubricas)
                    .HasForeignKey(d => d.IdAsignatura)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("rubricas_asignatura_fk");
                entity.HasOne(d => d.SO)
                    .WithMany(p => p.Rubricas)
                    .HasForeignKey(d => d.IdSO)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("rubricas_so_fk");
                entity.HasMany(d => d.Resumenes)
                    .WithOne(p => p.Rubrica)
                    .HasForeignKey(p => p.IdRubrica)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("resumenes_rubrica_fk");
                entity.HasMany(d => d.Evidencias)
                    .WithOne(p => p.Rubrica)
                    .HasForeignKey(p => p.IdRubrica)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("evidencias_rubrica_fk");
                entity.HasMany(d => d.CarreraRubricas)
                    .WithOne(p => p.Rubrica)
                    .HasForeignKey(p => p.IdRubrica)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("carrera_rubrica_fk");
                entity.HasMany(d => d.ActionPlans)
                    .WithOne(p => p.Rubrica)
                    .HasForeignKey(p => p.IdRubrica)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("actionplans_rubrica_fk");
            });

            // Asignatura
            modelBuilder.Entity<Asignatura>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Asignatura");
                entity.ToTable("asignaturas");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR AsignaturaSequence");
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UltimaEdicion).IsRequired(false);
                entity.Property(e => e.ProgramaAsignatura).IsRequired(false).HasMaxLength(500);
                entity.Property(e => e.Syllabus).IsRequired(false).HasMaxLength(500);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Asignaturas)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("asignaturas_ibfk_1");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Asignaturas)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("asignaturas_ibfk_2");
            });

            // Area
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Area");
                entity.ToTable("areas");
                entity.HasIndex(e => e.IdCoordinador, "Unique_Coordinador").IsUnique();
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR AreaSequence");
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UltimaEdicion).IsRequired(false);

                // Esta relación se mantiene Restrict
                entity.HasOne(d => d.Coordinador)
                    .WithOne()
                    .HasForeignKey<Area>(d => d.IdCoordinador)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("areas_ibfk_1")
                    .IsRequired(false);

                // Estas relaciones cambian a Restrict para evitar ciclos
                entity.HasMany(d => d.Usuarios)
                    .WithOne(p => p.Area)
                    .HasForeignKey(p => p.IdArea)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("areas_ibfk_2");

                entity.HasMany(d => d.Carreras)
                    .WithOne(p => p.Area)
                    .HasForeignKey(p => p.IdArea)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("areas_ibfk_3");

                entity.HasMany(d => d.Edificios)
                    .WithOne(p => p.Area)
                    .HasForeignKey(p => p.IdArea)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("areas_ibfk_4");

                entity.HasMany(d => d.Asignaturas)
                    .WithOne(p => p.Area)
                    .HasForeignKey(p => p.IdArea)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("areas_ibfk_5");
            });

            // Carrera
            modelBuilder.Entity<Carrera>(entity =>
            {
                // Clave primaria
                entity.HasKey(c => c.Id).HasName("PK_Carrera");

                entity.ToTable("carreras");
                // Configuración de propiedades
                entity.Property(c => c.NombreCarrera)
                    .IsRequired()
                    .HasMaxLength(255); // NombreCarrera como obligatorio y con longitud máxima


                entity.Property(c => c.Id).HasDefaultValueSql("NEXT VALUE FOR CarreraSequence"); // Id con valor por defecto
                entity.Property(c => c.PEOs)
                    .IsRequired(); // PEOs como obligatorio

                entity.Property(c => c.FechaCreacion)
                    .HasDefaultValueSql("GETDATE()"); // FechaCreacion con valor por defecto de la fecha actual

                entity.Property(c => c.UltimaEdicion)
                    .IsRequired(false); // UltimaEdicion puede ser nulo

                // Relaciones
                entity.HasOne(c => c.Area)
                    .WithMany()
                    .HasForeignKey(c => c.IdArea)
                    .OnDelete(DeleteBehavior.Restrict); // Relación con Area, sin eliminación en cascada
                entity.Property(c => c.IdCoordinadorCarrera).IsRequired(false);
                entity.HasOne(c => c.CoordinadorCarrera)
                    .WithMany()
                    .HasForeignKey(c => c.IdCoordinadorCarrera)
                    .OnDelete(DeleteBehavior.Restrict); // Relación con CoordinadorCarrera

                entity.HasOne(c => c.Estado)
                    .WithMany()
                    .HasForeignKey(c => c.IdEstado)
                    .OnDelete(DeleteBehavior.Restrict); // Relación con Estado

                // Relaciones de colecciones
                entity.HasMany(c => c.ProfesoresCarreras)
                    .WithOne()
                    .HasForeignKey(pc => pc.IdCarrera)
                    .OnDelete(DeleteBehavior.Cascade); // Relación con ProfesorCarrera

                entity.HasMany(c => c.Informes)
                    .WithOne()
                    .HasForeignKey(i => i.IdCarrera)
                    .OnDelete(DeleteBehavior.Cascade); // Relación con Informe

                entity.HasMany(c => c.CarreraRubricas)
                    .WithOne()
                    .HasForeignKey(cr => cr.IdCarrera)
                    .OnDelete(DeleteBehavior.Cascade); // Relación con CarreraRubrica

                entity.HasMany(c => c.AsignaturaCarreras)
                    .WithOne()
                    .HasForeignKey(ac => ac.IdCarrera)
                    .OnDelete(DeleteBehavior.Cascade); // Relación con AsignaturaCarrera

                // Índice único en NombreCarrera
                entity.HasIndex(c => c.NombreCarrera)
                    .IsUnique();
            });

            // Rol
            modelBuilder.Entity<Rol>(entity =>
            {
                // Clave primaria
                entity.HasKey(r => r.Id).HasName("PK_Rol");
                entity.ToTable("roles");

                entity.Property(r => r.Id).HasDefaultValueSql("NEXT VALUE FOR RolSequence"); // Id con valor por defecto
                // Configuración de propiedades
                entity.Property(r => r.Descripcion)
                    .IsRequired()
                    .HasMaxLength(255); // Descripcion es obligatoria y tiene longitud máxima de 255 caracteres

                // Propiedades booleanas con valores predeterminados
                entity.Property(r => r.EsProfesor)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.EsSupervisor)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.EsCoordinadorArea)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.EsCoordinadorCarrera)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.EsAdmin)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.EsAux)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.VerInformes)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.VerListaDeRubricas)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.ConfigurarFechas)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(r => r.VerManejoCurriculum)
                    .IsRequired()
                    .HasDefaultValue(false);

                // Relación con Usuario (uno a muchos)
                entity.HasMany(r => r.Usuarios)
                    .WithOne()
                    .HasForeignKey(u => u.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                // Índice único en la Descripción (opcional)
                entity.HasIndex(r => r.Descripcion)
                    .IsUnique(); // Asegura que no haya descripciones duplicadas
            });

            // PI
            modelBuilder.Entity<PI>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_PI");
                entity.ToTable("pi");
                entity.HasIndex(e => e.IdSO, "SO_Id");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR PISequence");
                entity.Property(e => e.IdSO).HasColumnName("SO_Id");
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
                entity.Property(e => e.DescripcionEN).IsRequired();
                entity.Property(e => e.DescripcionES).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UltimaEdicion);
                entity.HasOne(d => d.SO).WithMany(p => p.PIs)
                    .HasForeignKey(d => d.IdSO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pi_ibfk_1");
                entity.HasMany(d => d.Resumenes).WithOne(p => p.PI)
                    .HasForeignKey(d => d.IdPI)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pi_ibfk_2");
                entity.HasMany(d => d.Desempenos).WithOne(p => p.PI)
                   .HasForeignKey(d => d.IdPI)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("pi_ibfk_3");
            });

            // Informe
            modelBuilder.Entity<Informe>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Informe");
                entity.ToTable("informe");
                entity.HasIndex(e => e.IdTipo, "Tipo_Id");
                entity.HasIndex(e => e.IdCarrera, "Carrera_Id");

                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR InformeSequence");
                entity.Property(e => e.IdTipo).HasColumnName("Tipo_Id");
                entity.Property(e => e.IdCarrera).HasColumnName("Carrera_Id");
                entity.Property(e => e.Ruta).IsRequired().HasMaxLength(int.MaxValue);
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Año);
                entity.Property(e => e.Trimestre).HasColumnType("char(1)");
                entity.Property(e => e.Periodo).IsRequired();
                entity.HasOne(d => d.TipoInforme).WithMany(p => p.Informes)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("informe_ibfk_1");
                entity.HasOne(d => d.Carrera).WithMany(p => p.Informes)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("informe_ibfk_2");
            });

            // Edificio
            modelBuilder.Entity<Edificio>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Edificio");
                entity.ToTable("edificios");
                entity.HasIndex(e => e.IdArea, "Id_Area");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");

                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR EdificioSequence");
                entity.Property(e => e.IdArea).HasColumnName("Id_Area");
                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.Acron).IsRequired();
                entity.Property(e => e.Ubicacion).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.Area).WithMany(p => p.Edificios)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("edificio_ibfk_1");
                entity.HasOne(d => d.Estado).WithMany(p => p.Edificios)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("edificio_ibfk_2");
            });

            // ProfesorCarrera
            modelBuilder.Entity<ProfesorCarrera>(entity =>
            {
                entity.HasKey(e => new { e.IdProfesor, e.IdCarrera }).HasName("PK_ProfesorCarrera");
                entity.ToTable("profesor_carrera");
                entity.HasIndex(e => e.IdCarrera, "Carrera_Id");

                entity.Property(e => e.IdProfesor).HasColumnName("Profesor_Id");
                entity.Property(e => e.IdCarrera).HasColumnName("Carrera_Id");
                entity.HasOne(d => d.Carrera).WithMany(p => p.ProfesoresCarreras)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profesor_carrera_ibfk_1");
                entity.HasOne(d => d.Profesor).WithMany(p => p.ProfesoresCarreras)
                    .HasForeignKey(d => d.IdProfesor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profesor_carrera_ibfk_2");
            });

            // Desempeno
            modelBuilder.Entity<Desempeno>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Desempeno");
                entity.ToTable("desempeno");
                entity.HasIndex(e => e.IdSO, "Id_SO");
                entity.HasIndex(e => e.IdPI, "Id_PI");
                entity.HasIndex(e => e.IdAsignatura, "Id_Asignatura");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR DesempenoSequence");
                entity.Property(e => e.IdSO).HasColumnName("Id_SO");
                entity.Property(e => e.IdAsignatura).HasColumnName("Id_Asignatura");
                entity.Property(e => e.Satisfactorio).HasDefaultValue(false);
                entity.Property(e => e.Trimestre).HasColumnType("char(1)");
                entity.Property(e => e.Año).HasColumnType("smallint");
                entity.Property(e => e.Porcentaje).HasColumnType("decimal(5,2)");
                entity.HasOne(d => d.SO).WithMany(p => p.Desempenos)
                    .HasForeignKey(d => d.IdSO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("desempeno_ibfk_1");
                entity.HasOne(d => d.Asignatura).WithMany(p => p.Desempenos)
                    .HasForeignKey(d => d.IdAsignatura)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("desempeno_ibfk_2");
                entity.HasOne(d => d.PI).WithMany(p => p.Desempenos)
                    .HasForeignKey(d => d.IdPI)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("desempeno_ibfk_3");
            });

            // Contacto
            modelBuilder.Entity<Contacto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Contacto");
                entity.ToTable("contacto");
                entity.HasIndex(e => e.IdUsuario, "Id_Usuario");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR ContactoSequence");
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
                entity.Property(e => e.NumeroContacto).IsRequired();
                entity.HasOne(d => d.Usuario).WithMany(p => p.Contactos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contacto_ibfk_1");
            });


            // Evidencia
            modelBuilder.Entity<Evidencia>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Evidencia");
                entity.ToTable("evidencia");
                entity.HasIndex(e => e.IdRubrica, "Id_Rubrica");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR EvidenciaSequence");
                entity.Property(e => e.IdRubrica).HasColumnName("Id_Rubrica");
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.Ruta).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.Rubrica).WithMany(p => p.Evidencias)
                    .HasForeignKey(d => d.IdRubrica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("evidencia_ibfk_1");
            });


            // Aula
            modelBuilder.Entity<Aula>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Aula");
                entity.ToTable("aula");
                entity.HasIndex(e => e.IdEdificio, "Id_Edificio");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR AulaSequence");
                entity.Property(e => e.IdEdificio).HasColumnName("Id_Edificio");
                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.Edificio).WithMany(p => p.Aulas)
                    .HasForeignKey(d => d.IdEdificio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("aula_ibfk_1");
                entity.HasOne(d => d.Estado).WithMany(p => p.Aulas)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("aula_ibfk_2");
                entity.HasMany(d => d.ObjetosPorAula)
                    .WithOne(p => p.Aula)
                    .HasForeignKey(d => d.IdAula);
            });

            // Inventario
            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Inventario");
                entity.ToTable("inventario");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR InventarioSequence");
                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.Estado).WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("inventario_ibfk_1");
            });

            // Tarea
            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Tarea");
                entity.ToTable("tarea");
                entity.HasIndex(e => e.IdActionPlan, "Id_ActionPlan");
                entity.HasIndex(e => e.IdAuxiliar, "Id_Auxiliar");
                entity.HasIndex(e => e.IdEstadoTarea, "Id_EstadoTarea");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR TareaSequence");
                entity.Property(e => e.IdActionPlan).HasColumnName("Id_ActionPlan");
                entity.Property(e => e.IdAuxiliar).HasColumnName("Id_Auxiliar");
                entity.Property(e => e.IdEstadoTarea).HasColumnName("Id_EstadoTarea").HasDefaultValue(1);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.ActionPlan).WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.IdActionPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tarea_ibfk_1");
                entity.HasOne(d => d.Estado).WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.IdEstadoTarea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tarea_ibfk_2");
                entity.HasOne(d => d.Auxiliar).WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.IdAuxiliar)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tarea_ibfk_3");
            });

            // TipoCompetencia
            modelBuilder.Entity<TipoCompetencia>(entity =>
            {
                entity.HasIndex(e => e.Nombre, "Unique_Nombre").IsUnique();
                entity.HasKey(e => e.Id).HasName("PK_TipoCompetencia");
                entity.ToTable("tipo_competencia");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR TipoCompetenciaSequence");
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.HasMany(d => d.Competencias)
                    .WithOne(p => p.TipoCompetencia)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("competencia_ibfk_1");
            });

            // TipoInforme
            modelBuilder.Entity<TipoInforme>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TipoInforme");
                entity.ToTable("tipo_informe");
                entity.HasIndex(e => e.Descripcion, "Unique_Descripcion").IsUnique();
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR TipoInformeSequence");
                entity.Property(e => e.Descripcion).IsRequired();
                entity.HasMany(d => d.Informes)
                    .WithOne(p => p.TipoInforme)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("informe_ibfk_1");
            });

            // SOEvaluacion
            modelBuilder.Entity<SOEvaluacion>(entity =>
            {
                entity.HasKey(e => new { e.IdMetodoEvaluacion, e.IdSO }).HasName("PK_SOEvaluacion");
                entity.ToTable("so_evaluacion");
                entity.HasIndex(e => e.IdMetodoEvaluacion, "Id_MetodoEvaluacion");
                entity.HasIndex(e => e.IdSO, "Id_SO");
                entity.Property(e => e.IdMetodoEvaluacion).HasColumnName("Id_MetodoEvaluacion");
                entity.Property(e => e.IdSO).HasColumnName("Id_SO");
                entity.HasOne(d => d.MetodoEvaluacion).WithMany(p => p.SOEvaluaciones)
                    .HasForeignKey(d => d.IdMetodoEvaluacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("so_evaluacion_ibfk_1");
                entity.HasOne(d => d.SO).WithMany(p => p.SOEvaluaciones)
                    .HasForeignKey(d => d.IdSO)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("so_evaluacion_ibfk_2");
            });


            // ActionPlan
            modelBuilder.Entity<ActionPlan>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ActionPlan");
                entity.ToTable("action_plan");
                entity.HasIndex(e => e.IdDesempeno, "Id_Desempeno");
                entity.HasIndex(e => e.IdRubrica, "Id_Rubrica");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR ActionPlanSequence");
                entity.Property(e => e.IdDesempeno).HasColumnName("Id_Desempeno");
                entity.Property(e => e.IdRubrica).HasColumnName("Id_Rubrica");
                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado").HasDefaultValue(1);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.Desempeno).WithMany(p => p.ActionPlan)
                    .HasForeignKey(d => d.IdDesempeno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("action_plan_ibfk_1");
                entity.HasOne(d => d.Rubrica).WithMany(p => p.ActionPlans)
                    .HasForeignKey(d => d.IdRubrica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("action_plan_ibfk_2");
                entity.HasOne(d => d.Estado).WithMany(p => p.ActionPlans)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("action_plan_ibfk_3");
            });

            // Resumen
            modelBuilder.Entity<Resumen>(entity =>
            {
                entity.HasKey(e => new { e.IdPI, e.IdRubrica }).HasName("PK_Resumen");
                entity.ToTable("resumen");
                entity.HasIndex(e => e.IdPI, "Id_PI");
                entity.HasIndex(e => e.IdRubrica, "Id_Rubrica");
                entity.Property(e => e.IdPI).HasColumnName("Id_PI");
                entity.Property(e => e.IdRubrica).HasColumnName("Id_Rubrica");
                entity.Property(e => e.CantDesarrollo).HasDefaultValue(0);
                entity.Property(e => e.CantExperto).HasDefaultValue(0);
                entity.Property(e => e.CantPrincipiante).HasDefaultValue(0);
                entity.Property(e => e.CantSatisfactorio).HasDefaultValue(0);
                entity.HasOne(d => d.PI).WithMany(p => p.Resumenes)
                    .HasForeignKey(d => d.IdPI)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("resumen_ibfk_1");
                entity.HasOne(d => d.Rubrica).WithMany(p => p.Resumenes)
                    .HasForeignKey(d => d.IdRubrica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("resumen_ibfk_2");
            });

            // ConfiguracionEvaluaciones
            modelBuilder.Entity<ConfiguracionEvaluaciones>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Configuracion");
                entity.ToTable("ConfiguracionEvaluaciones");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR ConfiguracionSequence");
                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
                entity.Property(e => e.FechaInicio).IsRequired();
                entity.Property(e => e.FechaCierre).IsRequired();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(200);
                entity.HasOne(d => d.Estado).WithMany(p => p.Configuraciones)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("configuracion_ibfk_1");
            });

            // Competencia
            modelBuilder.Entity<Competencia>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Competencia");
                entity.ToTable("competencia");

                entity.HasIndex(e => e.Nombre, "Unique_Nombre").IsUnique();
                entity.HasIndex(e => e.Acron, "Unique_Acron").IsUnique(); // Índice único para los acrónimos (SO1, SO2, etc.)
                entity.HasIndex(e => e.IdTipo, "Id_Tipo");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");

                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR CompetenciaSequence");
                entity.Property(e => e.IdTipo).HasColumnName("Id_Tipo");
                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.Acron).IsRequired().HasMaxLength(10); // Limite de caracteres para acrónimo
                entity.Property(e => e.Titulo).IsRequired();
                entity.Property(e => e.DescripcionES).IsRequired();
                entity.Property(e => e.DescripcionEN).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UltimaEdicion).IsRequired(false);

                entity.HasOne(d => d.TipoCompetencia)
                    .WithMany(p => p.Competencias)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("competencia_ibfk_1");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Competencias)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("competencia_ibfk_2");

                entity.HasMany(d => d.MapaCompetencias)
                    .WithOne(p => p.Competencia)
                    .HasForeignKey(d => d.IdCompetencia);

                entity.HasMany(d => d.Rubricas)
                    .WithOne(p => p.SO)
                    .HasForeignKey(d => d.IdSO);

                entity.HasMany(d => d.SOEvaluaciones)
                    .WithOne(p => p.SO)
                    .HasForeignKey(d => d.IdSO);

                entity.HasMany(d => d.Desempenos)
                    .WithOne(p => p.SO)
                    .HasForeignKey(d => d.IdSO);

                entity.HasMany(d => d.PIs)
                    .WithOne(p => p.SO)
                    .HasForeignKey(d => d.IdSO);

                entity.HasMany(d => d.Profesores)
                    .WithOne(p => p.SO)
                    .HasForeignKey(d => d.IdSO);
            });


            // AsignaturaCarrera
            modelBuilder.Entity<AsignaturaCarrera>(entity =>
            {
                entity.HasKey(e => new { e.IdAsignatura, e.IdCarrera }).HasName("PK_AsignaturaCarrera");
                entity.ToTable("asignatura_carrera");
                entity.HasIndex(e => e.IdAsignatura, "Id_Asignatura");
                entity.HasIndex(e => e.IdCarrera, "Id_Carrera");
                entity.Property(e => e.IdAsignatura).HasColumnName("Id_Asignatura");
                entity.Property(e => e.IdCarrera).HasColumnName("Id_Carrera");
                entity.HasOne(d => d.Asignatura).WithMany(p => p.AsignaturaCarreras)
                    .HasForeignKey(d => d.IdAsignatura)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("asignatura_carrera_ibfk_1");
                entity.HasOne(d => d.Carrera).WithMany(p => p.AsignaturaCarreras)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("asignatura_carrera_ibfk_2");
            });

            // CarreraRubrica
            modelBuilder.Entity<CarreraRubrica>(entity =>
            {
                entity.HasKey(e => new { e.IdRubrica, e.IdCarrera }).HasName("PK_CarreraRubrica");
                entity.ToTable("carrera_rubrica");
                entity.HasIndex(e => e.IdRubrica, "Id_Rubrica");
                entity.HasIndex(e => e.IdCarrera, "Id_Carrera");
                entity.Property(e => e.IdRubrica).HasColumnName("Id_Rubrica");
                entity.Property(e => e.IdCarrera).HasColumnName("Id_Carrera");
                entity.HasOne(d => d.Rubrica).WithMany(p => p.CarreraRubricas)
                    .HasForeignKey(d => d.IdRubrica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("carrera_rubrica_ibfk_1");
                entity.HasOne(d => d.Carrera).WithMany(p => p.CarreraRubricas)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("carrera_rubrica_ibfk_2");
            });

            // HistorialIncumplimiento
            modelBuilder.Entity<HistorialIncumplimiento>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_HistorialIncumplimiento");
                entity.ToTable("historial_incumplimiento");
                entity.HasIndex(e => e.IdUsuario, "Id_Usuario");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR HistorialIncumplimientoSequence");
                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Fecha).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.Usuario).WithMany(p => p.HistorialIncumplimientos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("historial_incumplimiento_ibfk_1");
            });

            // MapaCompetencias
            modelBuilder.Entity<MapaCompetencias>(entity =>
            {
                entity.HasKey(e => new { e.IdAsignatura, e.IdCompetencia }).HasName("PK_MapaCompetencias");
                entity.ToTable("mapa_competencias");

                entity.HasIndex(e => e.IdAsignatura, "Id_Asignatura");
                entity.HasIndex(e => e.IdCompetencia, "Id_Competencia");
                entity.HasIndex(e => e.IdEstado, "Id_Estado");

                entity.Property(e => e.IdAsignatura).HasColumnName("Id_Asignatura");
                entity.Property(e => e.IdCompetencia).HasColumnName("Id_Competencia");
                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");

                entity.HasOne(d => d.Asignatura).WithMany(p => p.MapaCompetencias)
                    .HasForeignKey(d => d.IdAsignatura)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mapa_competencias_ibfk_1");

                entity.HasOne(d => d.Competencia).WithMany(p => p.MapaCompetencias)
                    .HasForeignKey(d => d.IdCompetencia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mapa_competencias_ibfk_2");

                entity.HasOne(d => d.Estado).WithMany(p => p.MapaCompetencias)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mapa_competencias_ibfk_3");
            });

            // MetodoEvaluacion
            modelBuilder.Entity<MetodoEvaluacion>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MetodoEvaluacion");
                entity.ToTable("metodo_evaluacion");
                entity.HasIndex(e => e.DescripcionES, "Unique_Descripcion").IsUnique();
                entity.HasIndex(e => e.DescripcionEN, "Unique_DescripcionEN").IsUnique();
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR MetodoEvaluacionSequence");
                entity.Property(e => e.DescripcionES).IsRequired();
                entity.Property(e => e.DescripcionEN).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UltimaEdicion).IsRequired(false);
                entity.HasMany(d => d.SOEvaluaciones)
                    .WithOne(p => p.MetodoEvaluacion)
                    .HasForeignKey(d => d.IdMetodoEvaluacion)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("so_evaluacion_metodo_fk");
                entity.HasMany(d => d.Rubricas)
                    .WithOne(p => p.MetodoEvaluacion)
                    .HasForeignKey(d => d.IdMetodoEvaluacion)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("rubricas_metodo_fk");
            });

            // ObjetosAula
            modelBuilder.Entity<ObjetoAula>(entity =>
            {
                entity.HasKey(e => new { e.IdObjeto, e.IdAula }).HasName("PK_ObjetoAula");
                entity.ToTable("objeto_aula");
                entity.HasIndex(e => e.IdObjeto, "Id_Objeto");
                entity.HasIndex(e => e.IdAula, "Id_Aula");
                entity.Property(e => e.IdObjeto).HasColumnName("Id_Objeto");
                entity.Property(e => e.IdAula).HasColumnName("Id_Aula");
                entity.Property(e => e.Cantidad).IsRequired();
                entity.HasOne(d => d.Aula).WithMany(p => p.ObjetosPorAula)
                    .HasForeignKey(d => d.IdAula)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("objeto_aula_ibfk_1");
                entity.HasOne(d => d.Inventario).WithMany(p => p.ObjetoPorAula)
                    .HasForeignKey(d => d.IdObjeto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("objeto_aula_ibfk_2");
            });

            // Estado
            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Estado");
                entity.ToTable("estado");
                entity.Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR EstadoSequence");
                entity.Property(e => e.IdTabla).IsRequired();
                entity.Property(e => e.Descripcion).IsRequired();

                entity.HasMany(d => d.Rubricas)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Usuarios)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Asignaturas)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Edificios)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Inventarios)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Tareas)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstadoTarea);

                entity.HasMany(d => d.ActionPlans)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Configuraciones)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Competencias)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.Aulas)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);

                entity.HasMany(d => d.MapaCompetencias)
                    .WithOne(p => p.Estado)
                    .HasForeignKey(d => d.IdEstado);
            });

        }
    }
}