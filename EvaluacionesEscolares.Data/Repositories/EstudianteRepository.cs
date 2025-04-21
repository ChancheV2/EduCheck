using EvaluacionesEscolares.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Data.Repositories
{
    public class EstudianteRepository : Repository<Estudiante>, IEstudianteRepository
    {
        public EstudianteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Estudiante>> GetEstudiantesConCalificacionesAsync()
        {
            return await _dbSet
                .Include(e => e.Calificaciones)
                .ThenInclude(c => c.Examen)
                .ThenInclude(e => e.Materia)
                .ToListAsync();
        }

        public async Task<Estudiante> GetEstudianteConCalificacionesAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Calificaciones)
                .ThenInclude(c => c.Examen)
                .ThenInclude(e => e.Materia)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }

    public interface IEstudianteRepository : IRepository<Estudiante>
    {
        Task<IEnumerable<Estudiante>> GetEstudiantesConCalificacionesAsync();
        Task<Estudiante> GetEstudianteConCalificacionesAsync(int id);
    }
}