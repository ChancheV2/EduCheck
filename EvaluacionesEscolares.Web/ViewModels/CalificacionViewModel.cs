using System.ComponentModel.DataAnnotations;

namespace EvaluacionesEscolares.Web.ViewModels
{
    public class CalificacionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El estudiante es obligatorio")]
        [Display(Name = "Estudiante")]
        public int EstudianteId { get; set; }

        [Display(Name = "Estudiante")]
        public string EstudianteNombre { get; set; }

        [Required(ErrorMessage = "El examen es obligatorio")]
        [Display(Name = "Examen")]
        public int ExamenId { get; set; }

        [Display(Name = "Examen")]
        public string ExamenTitulo { get; set; }

        [Display(Name = "Materia")]
        public string MateriaNombre { get; set; }

        [Required(ErrorMessage = "La nota es obligatoria")]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100")]
        [Display(Name = "Nota")]
        public decimal Nota { get; set; }

        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
    }
}