using EvaluacionesEscolares.Core.Services;
using EvaluacionesEscolares.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;
        private readonly ICalificacionService _calificacionService;

        public EstudiantesController(IEstudianteService estudianteService, ICalificacionService calificacionService)
        {
            _estudianteService = estudianteService;
            _calificacionService = calificacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            var estudiantes = await _estudianteService.GetAllEstudiantesAsync();
            return Ok(estudiantes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _estudianteService.GetEstudianteByIdAsync(id);
            if (estudiante == null)
                return NotFound();

            return Ok(estudiante);
        }

        [HttpGet("{id}/calificaciones")]
        public async Task<ActionResult<Estudiante>> GetEstudianteConCalificaciones(int id)
        {
            var estudiante = await _estudianteService.GetEstudianteConCalificacionesAsync(id);
            if (estudiante == null)
                return NotFound();

            return Ok(estudiante);
        }

        [HttpGet("{id}/promedios")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetPromediosPorMateria(int id)
        {
            var promedios = await _calificacionService.GetPromediosPorMateriaAsync(id);
            return Ok(promedios);
        }

        [HttpGet("{id}/promedio-general")]
        public async Task<ActionResult<decimal>> GetPromedioGeneral(int id)
        {
            var promedio = await _calificacionService.GetPromedioGeneralAsync(id);
            return Ok(promedio);
        }

        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
            await _estudianteService.CreateEstudianteAsync(estudiante);
            return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.Id }, estudiante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id)
                return BadRequest();

            try
            {
                await _estudianteService.UpdateEstudianteAsync(estudiante);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            try
            {
                await _estudianteService.DeleteEstudianteAsync(id);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}