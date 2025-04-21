namespace EvaluacionesEscolares.Entities
{
    public class Opcion
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public bool EsCorrecta { get; set; }
        public int PreguntaId { get; set; }
        public virtual Pregunta Pregunta { get; set; }
        public string TextoOpcion { get; set; }
    }
}