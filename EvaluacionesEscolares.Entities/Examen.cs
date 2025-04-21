using System;
using System.Collections.Generic;

namespace EvaluacionesEscolares.Entities
{
    public class Examen
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public int DuracionMinutos { get; set; }
        public int MateriaId { get; set; }
        public virtual Materia Materia { get; set; }
        public TipoExamen Tipo { get; set; }
        public virtual ICollection<Pregunta> Preguntas { get; set; }
        public virtual ICollection<Calificacion> Calificaciones { get; set; }
        public int Duracion { get; set; }
    }

    public enum TipoExamen
    {
        Parcial1,
        Parcial2,
        ExamenFinal
    }
}