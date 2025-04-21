using EvaluacionesEscolares.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionesEscolares.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Examen>()
                .HasOne(e => e.Materia)
                .WithMany(m => m.Examenes)
                .HasForeignKey(e => e.MateriaId);

            modelBuilder.Entity<Pregunta>()
                .HasOne(p => p.Examen)
                .WithMany(e => e.Preguntas)
                .HasForeignKey(p => p.ExamenId);

            modelBuilder.Entity<Opcion>()
                .HasOne(o => o.Pregunta)
                .WithMany(p => p.Opciones)
                .HasForeignKey(o => o.PreguntaId);

            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.Estudiante)
                .WithMany(e => e.Calificaciones)
                .HasForeignKey(c => c.EstudianteId);

            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.Examen)
                .WithMany(e => e.Calificaciones)
                .HasForeignKey(c => c.ExamenId);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Materia>().HasData(
                new Materia { Id = 1, Nombre = "Lengua Española", Descripcion = "Estudio del idioma español y su literatura" },
                new Materia { Id = 2, Nombre = "Matemática", Descripcion = "Estudio de números, cantidades y formas" },
                new Materia { Id = 3, Nombre = "Biología", Descripcion = "Estudio de los seres vivos" },
                new Materia { Id = 4, Nombre = "Física", Descripcion = "Estudio de la materia y la energía" },
                new Materia { Id = 5, Nombre = "Química", Descripcion = "Estudio de la composición de la materia" },
                new Materia { Id = 6, Nombre = "Geografía", Descripcion = "Estudio de la Tierra y sus características" },
                new Materia { Id = 7, Nombre = "Historia", Descripcion = "Estudio de eventos pasados" },
                new Materia { Id = 8, Nombre = "Educación Física", Descripcion = "Desarrollo físico y deportivo" },
                new Materia { Id = 9, Nombre = "Lengua Extranjera", Descripcion = "Estudio de idiomas extranjeros" }
            );
        }
    }
}