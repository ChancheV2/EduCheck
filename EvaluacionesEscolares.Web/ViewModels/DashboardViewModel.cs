namespace EvaluacionesEscolares.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEstudiantes { get; set; }
        public int TotalExamenes { get; set; }
        public int TotalMaterias { get; set; }
        public Dictionary<string, decimal> PromediosPorMateria { get; set; } = new Dictionary<string, decimal>();
        public List<EstudianteViewModel> UltimosEstudiantes { get; set; } = new List<EstudianteViewModel>();
        public List<ExamenViewModel> UltimosExamenes { get; set; } = new List<ExamenViewModel>();
        public List<CalificacionViewModel> UltimasCalificaciones { get; set; } = new List<CalificacionViewModel>();
    }
}