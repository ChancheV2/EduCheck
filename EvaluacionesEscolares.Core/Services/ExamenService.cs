using EvaluacionesEscolares.Data.Repositories;
using EvaluacionesEscolares.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.Core.Services
{
    public class ExamenService : IExamenService
    {
        private readonly IExamenRepository _examenRepository;
        private readonly ILogger<ExamenService> _logger;

        public ExamenService(IExamenRepository examenRepository, ILogger<ExamenService> logger)
        {
            _examenRepository = examenRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Examen>> GetAllExamenesAsync()
        {
            return await _examenRepository.GetAllAsync();
        }

        public async Task<Examen> GetExamenByIdAsync(int id)
        {
            return await _examenRepository.GetByIdAsync(id);
        }

        public async Task<Examen> GetExamenCompletoAsync(int id)
        {
            return await _examenRepository.GetExamenCompletoAsync(id);
        }

        public async Task<IEnumerable<Examen>> GetExamenesCompletosAsync()
        {
            return await _examenRepository.GetExamenesCompletosAsync();
        }

        public async Task<IEnumerable<Examen>> GetExamenesPorMateriaAsync(int materiaId)
        {
            return await _examenRepository.GetExamenesPorMateriaAsync(materiaId);
        }

        public async Task<Examen> CreateExamenAsync(Examen examen)
        {
            try
            {
                _logger.LogInformation("Iniciando creación de examen: {Titulo}, MateriaId: {MateriaId}",
                    examen?.Titulo, examen?.MateriaId);

                if (examen == null)
                {
                    _logger.LogError("Error al crear examen: El objeto examen es nulo");
                    throw new ArgumentNullException(nameof(examen));
                }

                examen.FechaCreacion = DateTime.Now;

                _logger.LogInformation("Llamando a AddAsync en el repositorio");
                var addedExamen = await _examenRepository.AddAsync(examen);

                _logger.LogInformation("Llamando a SaveChangesAsync en el repositorio");
                var result = await _examenRepository.SaveChangesAsync();

                _logger.LogInformation("Examen creado con ID {Id}, SaveChanges devolvió {Result}",
                    addedExamen.Id, result);

                return addedExamen;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el examen: {Message}", ex.Message);
                throw; // Re-lanzar la excepción para que sea manejada por el controlador
            }
        }

        public async Task UpdateExamenAsync(Examen examen)
        {
            try
            {
                _logger.LogInformation("Iniciando actualización de examen ID: {Id}", examen?.Id);

                if (examen == null)
                {
                    _logger.LogError("Error al actualizar examen: El objeto examen es nulo");
                    throw new ArgumentNullException(nameof(examen));
                }

                await _examenRepository.UpdateAsync(examen);
                var result = await _examenRepository.SaveChangesAsync();

                _logger.LogInformation("Examen actualizado, SaveChanges devolvió {Result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el examen: {Message}", ex.Message);
                throw;
            }
        }

        public async Task DeleteExamenAsync(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando eliminación de examen ID: {Id}", id);

                var examen = await _examenRepository.GetByIdAsync(id);
                if (examen == null)
                {
                    _logger.LogError("Error al eliminar examen: Examen con ID {Id} no encontrado", id);
                    throw new ArgumentException($"Examen con ID {id} no encontrado");
                }

                await _examenRepository.DeleteAsync(examen);
                var result = await _examenRepository.SaveChangesAsync();

                _logger.LogInformation("Examen eliminado, SaveChanges devolvió {Result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el examen: {Message}", ex.Message);
                throw;
            }
        }
    }
}