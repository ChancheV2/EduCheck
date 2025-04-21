using EvaluacionesEscolares.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Core.Services
{
    public interface IExamenService
    {
        Task<IEnumerable<Examen>> GetAllExamenesAsync();
        Task<Examen> GetExamenByIdAsync(int id);
        Task<Examen> GetExamenCompletoAsync(int id);
        Task<IEnumerable<Examen>> GetExamenesCompletosAsync();
        Task<IEnumerable<Examen>> GetExamenesPorMateriaAsync(int materiaId);
        Task<Examen> CreateExamenAsync(Examen examen);
        Task UpdateExamenAsync(Examen examen);
        Task DeleteExamenAsync(int id);
    }
}