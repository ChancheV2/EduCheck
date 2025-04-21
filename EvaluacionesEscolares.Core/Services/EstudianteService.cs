using EvaluacionesEscolares.Data.Repositories;
using EvaluacionesEscolares.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Core.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IEstudianteRepository _estudianteRepository;

        public EstudianteService(IEstudianteRepository estudianteRepository)
        {
            _estudianteRepository = estudianteRepository;
        }

        public async Task<IEnumerable<Estudiante>> GetAllEstudiantesAsync()
        {
            return await _estudianteRepository.GetAllAsync();
        }

        public async Task<Estudiante> GetEstudianteByIdAsync(int id)
        {
            return await _estudianteRepository.GetByIdAsync(id);
        }

        public async Task<Estudiante> GetEstudianteConCalificacionesAsync(int id)
        {
            return await _estudianteRepository.GetEstudianteConCalificacionesAsync(id);
        }

        public async Task<IEnumerable<Estudiante>> GetEstudiantesConCalificacionesAsync()
        {
            return await _estudianteRepository.GetEstudiantesConCalificacionesAsync();
        }

        public async Task<Estudiante> CreateEstudianteAsync(Estudiante estudiante)
        {
            if (estudiante == null)
                throw new ArgumentNullException(nameof(estudiante));

            await _estudianteRepository.AddAsync(estudiante);
            await _estudianteRepository.SaveChangesAsync();
            return estudiante;
        }

        public async Task UpdateEstudianteAsync(Estudiante estudiante)
        {
            if (estudiante == null)
                throw new ArgumentNullException(nameof(estudiante));

            await _estudianteRepository.UpdateAsync(estudiante);
            await _estudianteRepository.SaveChangesAsync();
        }

        public async Task DeleteEstudianteAsync(int id)
        {
            var estudiante = await _estudianteRepository.GetByIdAsync(id);
            if (estudiante == null)
                throw new ArgumentException($"Estudiante con ID {id} no encontrado");

            await _estudianteRepository.DeleteAsync(estudiante);
            await _estudianteRepository.SaveChangesAsync();
        }
    }
}