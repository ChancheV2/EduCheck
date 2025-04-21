using System.ComponentModel.DataAnnotations;

namespace EvaluacionesEscolares.Web.ViewModels
{
    public class OpcionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El texto de la opción es obligatorio")]
        [Display(Name = "Texto de la Opción")]
        public string TextoOpcion { get; set; }

        [Display(Name = "Es Correcta")]
        public bool EsCorrecta { get; set; }

        [Required(ErrorMessage = "La pregunta es obligatoria")]
        public int PreguntaId { get; set; }
    }
}