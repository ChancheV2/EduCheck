using EvaluacionesEscolares.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Data.Repositories
{
    public class CalificacionRepository : Repository<Calificacion>, ICalificacionRepository
    {
        public CalificacionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Calificacion>> GetCalificacionesPorEstudianteAsync(int estudianteId)
        {
            return await _dbSet
                .Include(c => c.Examen)
                .ThenInclude(e => e.Materia)
                .Where(c => c.EstudianteId == estudianteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Calificacion>> GetCalificacionesPorMateriaAsync(int materiaId)
        {
            return await _dbSet
                .Include(c => c.Estudiante)
                .Include(c => c.Examen)
                .Where(c => c.Examen.MateriaId == materiaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Calificacion>> GetCalificacionesPorExamenAsync(int examenId)
        {
            return await _dbSet
                .Include(c => c.Estudiante)
                .Where(c => c.ExamenId == examenId)
                .ToListAsync();
        }
    }

    public interface ICalificacionRepository : IRepository<Calificacion>
    {
        Task<IEnumerable<Calificacion>> GetCalificacionesPorEstudianteAsync(int estudianteId);
        Task<IEnumerable<Calificacion>> GetCalificacionesPorMateriaAsync(int materiaId);
        Task<IEnumerable<Calificacion>> GetCalificacionesPorExamenAsync(int examenId);
    }
}