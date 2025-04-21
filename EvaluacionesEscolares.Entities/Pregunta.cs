using System.Collections.Generic;

namespace EvaluacionesEscolares.Entities
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public int Puntos { get; set; }
        public int ExamenId { get; set; }
        public virtual Examen Examen { get; set; }
        public virtual ICollection<Opcion> Opciones { get; set; }
        public string TextoPregunta { get; set; }
    }
}