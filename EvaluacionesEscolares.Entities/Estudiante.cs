using System;
using System.Collections.Generic;

namespace EvaluacionesEscolares.Entities
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public virtual ICollection<Calificacion> Calificaciones { get; set; }
    }
}