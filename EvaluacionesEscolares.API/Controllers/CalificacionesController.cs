using EvaluacionesEscolares.Core.Services;
using EvaluacionesEscolares.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionService _calificacionService;

        public CalificacionesController(ICalificacionService calificacionService)
        {
            _calificacionService = calificacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calificacion>>> GetCalificaciones()
        {
            var calificaciones = await _calificacionService.GetAllCalificacionesAsync();
            return Ok(calificaciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Calificacion>> GetCalificacion(int id)
        {
            var calificacion = await _calificacionService.GetCalificacionByIdAsync(id);
            if (calificacion == null)
                return NotFound();

            return Ok(calificacion);
        }

        [HttpGet("estudiante/{estudianteId}")]
        public async Task<ActionResult<IEnumerable<Calificacion>>> GetCalificacionesPorEstudiante(int estudianteId)
        {
            var calificaciones = await _calificacionService.GetCalificacionesPorEstudianteAsync(estudianteId);
            return Ok(calificaciones);
        }

        [HttpGet("materia/{materiaId}")]
        public async Task<ActionResult<IEnumerable<Calificacion>>> GetCalificacionesPorMateria(int materiaId)
        {
            var calificaciones = await _calificacionService.GetCalificacionesPorMateriaAsync(materiaId);
            return Ok(calificaciones);
        }

        [HttpGet("examen/{examenId}")]
        public async Task<ActionResult<IEnumerable<Calificacion>>> GetCalificacionesPorExamen(int examenId)
        {
            var calificaciones = await _calificacionService.GetCalificacionesPorExamenAsync(examenId);
            return Ok(calificaciones);
        }

        [HttpPost]
        public async Task<ActionResult<Calificacion>> PostCalificacion(Calificacion calificacion)
        {
            await _calificacionService.CreateCalificacionAsync(calificacion);
            return CreatedAtAction(nameof(GetCalificacion), new { id = calificacion.Id }, calificacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalificacion(int id, Calificacion calificacion)
        {
            if (id != calificacion.Id)
                return BadRequest();

            try
            {
                await _calificacionService.UpdateCalificacionAsync(calificacion);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalificacion(int id)
        {
            try
            {
                await _calificacionService.DeleteCalificacionAsync(id);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}