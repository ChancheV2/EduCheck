using System.ComponentModel.DataAnnotations;

namespace EvaluacionesEscolares.Web.ViewModels
{
    public class PreguntaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El texto de la pregunta es obligatorio")]
        [Display(Name = "Texto de la Pregunta")]
        public string TextoPregunta { get; set; }

        [Required(ErrorMessage = "El valor de puntos es obligatorio")]
        [Range(1, 100, ErrorMessage = "El valor debe estar entre 1 y 100 puntos")]
        [Display(Name = "Puntos")]
        public int Puntos { get; set; }

        [Required(ErrorMessage = "El examen es obligatorio")]
        public int ExamenId { get; set; }

        [Display(Name = "Opciones")]
        public List<OpcionViewModel> Opciones { get; set; } = new List<OpcionViewModel>();
    }
}