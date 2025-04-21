using EvaluacionesEscolares.Core.Services;
using EvaluacionesEscolares.Entities;
using EvaluacionesEscolares.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EvaluacionesEscolares.Web.Controllers
{
    public class CalificacionesController : Controller
    {
        private readonly ICalificacionService _calificacionService;
        private readonly IEstudianteService _estudianteService;
        private readonly IExamenService _examenService;
        private readonly ILogger<CalificacionesController> _logger;

        public CalificacionesController(
            ICalificacionService calificacionService,
            IEstudianteService estudianteService,
            IExamenService examenService,
            ILogger<CalificacionesController> logger)
        {
            _calificacionService = calificacionService;
            _estudianteService = estudianteService;
            _examenService = examenService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var calificaciones = await _calificacionService.GetAllCalificacionesAsync();
                var viewModels = new List<CalificacionViewModel>();

                foreach (var calificacion in calificaciones)
                {
                    var estudiante = await _estudianteService.GetEstudianteByIdAsync(calificacion.EstudianteId);
                    var examen = await _examenService.GetExamenByIdAsync(calificacion.ExamenId);

                    viewModels.Add(new CalificacionViewModel
                    {
                        Id = calificacion.Id,
                        EstudianteId = calificacion.EstudianteId,
                        EstudianteNombre = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellido}" : "Desconocido",
                        ExamenId = calificacion.ExamenId,
                        ExamenTitulo = examen?.Titulo ?? "Desconocido",
                        MateriaNombre = examen?.Materia?.Nombre ?? "Desconocida",
                        Nota = calificacion.Nota,
                        Comentarios = calificacion.Comentarios,
                        FechaRegistro = calificacion.FechaRegistro
                    });
                }

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de calificaciones");
                TempData["Error"] = "Ocurrió un error al cargar las calificaciones.";
                return View(new List<CalificacionViewModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var calificacion = await _calificacionService.GetCalificacionByIdAsync(id);
                if (calificacion == null)
                {
                    return NotFound();
                }

                var estudiante = await _estudianteService.GetEstudianteByIdAsync(calificacion.EstudianteId);
                var examen = await _examenService.GetExamenByIdAsync(calificacion.ExamenId);

                var viewModel = new CalificacionViewModel
                {
                    Id = calificacion.Id,
                    EstudianteId = calificacion.EstudianteId,
                    EstudianteNombre = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellido}" : "Desconocido",
                    ExamenId = calificacion.ExamenId,
                    ExamenTitulo = examen?.Titulo ?? "Desconocido",
                    MateriaNombre = examen?.Materia?.Nombre ?? "Desconocida",
                    Nota = calificacion.Nota,
                    Comentarios = calificacion.Comentarios,
                    FechaRegistro = calificacion.FechaRegistro
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles de la calificación");
                TempData["Error"] = "Ocurrió un error al cargar los detalles de la calificación.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var estudiantes = await _estudianteService.GetAllEstudiantesAsync();
                var examenes = await _examenService.GetExamenesCompletosAsync();

                ViewBag.Estudiantes = new SelectList(estudiantes, "Id", "NombreCompleto");
                ViewBag.Examenes = new SelectList(examenes, "Id", "Titulo");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el formulario de creación de calificación");
                TempData["Error"] = "Ocurrió un error al cargar el formulario.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CalificacionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var calificacion = new Calificacion
                    {
                        EstudianteId = viewModel.EstudianteId,
                        ExamenId = viewModel.ExamenId,
                        Nota = viewModel.Nota,
                        Comentarios = viewModel.Comentarios,
                        FechaRegistro = DateTime.Now
                    };

                    await _calificacionService.CreateCalificacionAsync(calificacion);
                    TempData["Success"] = "Calificación creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear la calificación");
                    ModelState.AddModelError("", "Ocurrió un error al crear la calificación.");
                }
            }

            var estudiantes = await _estudianteService.GetAllEstudiantesAsync();
            var examenes = await _examenService.GetExamenesCompletosAsync();

            ViewBag.Estudiantes = new SelectList(estudiantes, "Id", "NombreCompleto", viewModel.EstudianteId);
            ViewBag.Examenes = new SelectList(examenes, "Id", "Titulo", viewModel.ExamenId);

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var calificacion = await _calificacionService.GetCalificacionByIdAsync(id);
                if (calificacion == null)
                {
                    return NotFound();
                }

                var estudiantes = await _estudianteService.GetAllEstudiantesAsync();
                var examenes = await _examenService.GetExamenesCompletosAsync();

                ViewBag.Estudiantes = new SelectList(estudiantes, "Id", "NombreCompleto", calificacion.EstudianteId);
                ViewBag.Examenes = new SelectList(examenes, "Id", "Titulo", calificacion.ExamenId);

                var viewModel = new CalificacionViewModel
                {
                    Id = calificacion.Id,
                    EstudianteId = calificacion.EstudianteId,
                    ExamenId = calificacion.ExamenId,
                    Nota = calificacion.Nota,
                    Comentarios = calificacion.Comentarios,
                    FechaRegistro = calificacion.FechaRegistro
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la calificación para editar");
                TempData["Error"] = "Ocurrió un error al cargar la calificación.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CalificacionViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var calificacion = await _calificacionService.GetCalificacionByIdAsync(id);
                    if (calificacion == null)
                    {
                        return NotFound();
                    }

                    calificacion.EstudianteId = viewModel.EstudianteId;
                    calificacion.ExamenId = viewModel.ExamenId;
                    calificacion.Nota = viewModel.Nota;
                    calificacion.Comentarios = viewModel.Comentarios;

                    await _calificacionService.UpdateCalificacionAsync(calificacion);
                    TempData["Success"] = "Calificación actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar la calificación");
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la calificación.");
                }
            }

            var estudiantes = await _estudianteService.GetAllEstudiantesAsync();
            var examenes = await _examenService.GetExamenesCompletosAsync();

            ViewBag.Estudiantes = new SelectList(estudiantes, "Id", "NombreCompleto", viewModel.EstudianteId);
            ViewBag.Examenes = new SelectList(examenes, "Id", "Titulo", viewModel.ExamenId);

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var calificacion = await _calificacionService.GetCalificacionByIdAsync(id);
                if (calificacion == null)
                {
                    return NotFound();
                }

                var estudiante = await _estudianteService.GetEstudianteByIdAsync(calificacion.EstudianteId);
                var examen = await _examenService.GetExamenByIdAsync(calificacion.ExamenId);

                var viewModel = new CalificacionViewModel
                {
                    Id = calificacion.Id,
                    EstudianteId = calificacion.EstudianteId,
                    EstudianteNombre = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellido}" : "Desconocido",
                    ExamenId = calificacion.ExamenId,
                    ExamenTitulo = examen?.Titulo ?? "Desconocido",
                    MateriaNombre = examen?.Materia?.Nombre ?? "Desconocida",
                    Nota = calificacion.Nota,
                    Comentarios = calificacion.Comentarios,
                    FechaRegistro = calificacion.FechaRegistro
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la calificación para eliminar");
                TempData["Error"] = "Ocurrió un error al cargar la calificación.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _calificacionService.DeleteCalificacionAsync(id);
                TempData["Success"] = "Calificación eliminada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la calificación");
                TempData["Error"] = "Ocurrió un error al eliminar la calificación.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}