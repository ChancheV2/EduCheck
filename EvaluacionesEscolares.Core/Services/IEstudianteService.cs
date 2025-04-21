using EvaluacionesEscolares.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Core.Services
{
    public interface IEstudianteService
    {
        Task<IEnumerable<Estudiante>> GetAllEstudiantesAsync();
        Task<Estudiante> GetEstudianteByIdAsync(int id);
        Task<Estudiante> GetEstudianteConCalificacionesAsync(int id);
        Task<IEnumerable<Estudiante>> GetEstudiantesConCalificacionesAsync();
        Task<Estudiante> CreateEstudianteAsync(Estudiante estudiante);
        Task UpdateEstudianteAsync(Estudiante estudiante);
        Task DeleteEstudianteAsync(int id);
    }
}