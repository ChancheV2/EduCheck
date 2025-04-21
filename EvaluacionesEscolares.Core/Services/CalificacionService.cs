using EvaluacionesEscolares.Data.Repositories;
using EvaluacionesEscolares.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Core.Services
{
    public class CalificacionService : ICalificacionService
    {
        private readonly ICalificacionRepository _calificacionRepository;

        public CalificacionService(ICalificacionRepository calificacionRepository)
        {
            _calificacionRepository = calificacionRepository;
        }

        public async Task<IEnumerable<Calificacion>> GetAllCalificacionesAsync()
        {
            return await _calificacionRepository.GetAllAsync();
        }

        public async Task<Calificacion> GetCalificacionByIdAsync(int id)
        {
            return await _calificacionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Calificacion>> GetCalificacionesPorEstudianteAsync(int estudianteId)
        {
            return await _calificacionRepository.GetCalificacionesPorEstudianteAsync(estudianteId);
        }

        public async Task<IEnumerable<Calificacion>> GetCalificacionesPorMateriaAsync(int materiaId)
        {
            return await _calificacionRepository.GetCalificacionesPorMateriaAsync(materiaId);
        }

        public async Task<IEnumerable<Calificacion>> GetCalificacionesPorExamenAsync(int examenId)
        {
            return await _calificacionRepository.GetCalificacionesPorExamenAsync(examenId);
        }

        public async Task<Calificacion> CreateCalificacionAsync(Calificacion calificacion)
        {
            if (calificacion == null)
                throw new ArgumentNullException(nameof(calificacion));

            calificacion.FechaRegistro = DateTime.Now;
            await _calificacionRepository.AddAsync(calificacion);
            await _calificacionRepository.SaveChangesAsync();
            return calificacion;
        }

        public async Task UpdateCalificacionAsync(Calificacion calificacion)
        {
            if (calificacion == null)
                throw new ArgumentNullException(nameof(calificacion));

            await _calificacionRepository.UpdateAsync(calificacion);
            await _calificacionRepository.SaveChangesAsync();
        }

        public async Task DeleteCalificacionAsync(int id)
        {
            var calificacion = await _calificacionRepository.GetByIdAsync(id);
            if (calificacion == null)
                throw new ArgumentException($"Calificación con ID {id} no encontrada");

            await _calificacionRepository.DeleteAsync(calificacion);
            await _calificacionRepository.SaveChangesAsync();
        }

        public async Task<Dictionary<string, decimal>> GetPromediosPorMateriaAsync(int estudianteId)
        {
            var calificaciones = await _calificacionRepository.GetCalificacionesPorEstudianteAsync(estudianteId);

            return calificaciones
                .GroupBy(c => c.Examen.Materia.Nombre)
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(c => c.Nota)
                );
        }

        public async Task<decimal> GetPromedioGeneralAsync(int estudianteId)
        {
            var calificaciones = await _calificacionRepository.GetCalificacionesPorEstudianteAsync(estudianteId);

            if (!calificaciones.Any())
                return 0;

            return calificaciones.Average(c => c.Nota);
        }
    }
}