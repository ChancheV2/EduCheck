using System.ComponentModel.DataAnnotations;

namespace EvaluacionesEscolares.Web.ViewModels
{
    public class ExamenViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 300, ErrorMessage = "La duración debe estar entre 1 y 300 minutos")]
        [Display(Name = "Duración (minutos)")]
        public int Duracion { get; set; }

        [Required(ErrorMessage = "La materia es obligatoria")]
        [Display(Name = "Materia")]
        public int MateriaId { get; set; }

        [Display(Name = "Materia")]
        public string MateriaNombre { get; set; }

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Preguntas")]
        public List<PreguntaViewModel> Preguntas { get; set; } = new List<PreguntaViewModel>();
    }
}