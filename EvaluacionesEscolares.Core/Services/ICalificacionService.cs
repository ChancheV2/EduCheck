using EvaluacionesEscolares.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Core.Services
{
    public interface ICalificacionService
    {
        Task<IEnumerable<Calificacion>> GetAllCalificacionesAsync();
        Task<Calificacion> GetCalificacionByIdAsync(int id);
        Task<IEnumerable<Calificacion>> GetCalificacionesPorEstudianteAsync(int estudianteId);
        Task<IEnumerable<Calificacion>> GetCalificacionesPorMateriaAsync(int materiaId);
        Task<IEnumerable<Calificacion>> GetCalificacionesPorExamenAsync(int examenId);
        Task<Calificacion> CreateCalificacionAsync(Calificacion calificacion);
        Task UpdateCalificacionAsync(Calificacion calificacion);
        Task DeleteCalificacionAsync(int id);
        Task<Dictionary<string, decimal>> GetPromediosPorMateriaAsync(int estudianteId);
        Task<decimal> GetPromedioGeneralAsync(int estudianteId);
    }
}