using System;

namespace EvaluacionesEscolares.Entities
{
    public class Calificacion
    {
        public int Id { get; set; }
        public decimal Nota { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int EstudianteId { get; set; }
        public virtual Estudiante Estudiante { get; set; }
        public int ExamenId { get; set; }
        public virtual Examen Examen { get; set; }
        public string Comentarios { get; set; }
    }
}