using EvaluacionesEscolares.Core.Services;
using EvaluacionesEscolares.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaluacionesEscolares.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenesController : ControllerBase
    {
        private readonly IExamenService _examenService;

        public ExamenesController(IExamenService examenService)
        {
            _examenService = examenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Examen>>> GetExamenes()
        {
            var examenes = await _examenService.GetAllExamenesAsync();
            return Ok(examenes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Examen>> GetExamen(int id)
        {
            var examen = await _examenService.GetExamenByIdAsync(id);
            if (examen == null)
                return NotFound();

            return Ok(examen);
        }

        [HttpGet("completo/{id}")]
        public async Task<ActionResult<Examen>> GetExamenCompleto(int id)
        {
            var examen = await _examenService.GetExamenCompletoAsync(id);
            if (examen == null)
                return NotFound();

            return Ok(examen);
        }

        [HttpGet("materia/{materiaId}")]
        public async Task<ActionResult<IEnumerable<Examen>>> GetExamenesPorMateria(int materiaId)
        {
            var examenes = await _examenService.GetExamenesPorMateriaAsync(materiaId);
            return Ok(examenes);
        }

        [HttpPost]
        public async Task<ActionResult<Examen>> PostExamen(Examen examen)
        {
            await _examenService.CreateExamenAsync(examen);
            return CreatedAtAction(nameof(GetExamen), new { id = examen.Id }, examen);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamen(int id, Examen examen)
        {
            if (id != examen.Id)
                return BadRequest();

            try
            {
                await _examenService.UpdateExamenAsync(examen);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamen(int id)
        {
            try
            {
                await _examenService.DeleteExamenAsync(id);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}