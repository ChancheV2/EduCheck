using EvaluacionesEscolares.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Data.Repositories
{
    public class ExamenRepository : Repository<Examen>, IExamenRepository
    {
        public ExamenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Examen>> GetExamenesCompletosAsync()
        {
            return await _dbSet
                .Include(e => e.Materia)
                .Include(e => e.Preguntas)
                .ThenInclude(p => p.Opciones)
                .ToListAsync();
        }

        public async Task<Examen> GetExamenCompletoAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Materia)
                .Include(e => e.Preguntas)
                .ThenInclude(p => p.Opciones)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Examen>> GetExamenesPorMateriaAsync(int materiaId)
        {
            return await _dbSet
                .Include(e => e.Materia)
                .Where(e => e.MateriaId == materiaId)
                .ToListAsync();
        }
    }

    public interface IExamenRepository : IRepository<Examen>
    {
        Task<IEnumerable<Examen>> GetExamenesCompletosAsync();
        Task<Examen> GetExamenCompletoAsync(int id);
        Task<IEnumerable<Examen>> GetExamenesPorMateriaAsync(int materiaId);
    }
}