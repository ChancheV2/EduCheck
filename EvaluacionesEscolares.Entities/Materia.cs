using System.Collections.Generic;

namespace EvaluacionesEscolares.Entities
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<Examen> Examenes { get; set; }
    }
}