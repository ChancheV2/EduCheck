using EvaluacionesEscolares.Core.Services;
using EvaluacionesEscolares.Entities;
using EvaluacionesEscolares.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EvaluacionesEscolares.Web.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly IEstudianteService _estudianteService;
        private readonly ICalificacionService _calificacionService;
        private readonly ILogger<EstudiantesController> _logger;

        public EstudiantesController(
            IEstudianteService estudianteService,
            ICalificacionService calificacionService,
            ILogger<EstudiantesController> logger)
        {
            _estudianteService = estudianteService;
            _calificacionService = calificacionService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var estudiantes = await _estudianteService.GetAllEstudiantesAsync();
                var viewModels = estudiantes.Select(e => new EstudianteViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    Matricula = e.Matricula,
                    Email = e.Email,
                    FechaNacimiento = e.FechaNacimiento
                }).ToList();

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de estudiantes");
                TempData["Error"] = "Ocurrió un error al cargar los estudiantes.";
                return View(new List<EstudianteViewModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var estudiante = await _estudianteService.GetEstudianteConCalificacionesAsync(id);
                if (estudiante == null)
                {
                    return NotFound();
                }

                var promedios = await _calificacionService.GetPromediosPorMateriaAsync(id);
                var promedioGeneral = await _calificacionService.GetPromedioGeneralAsync(id);

                ViewBag.Promedios = promedios;
                ViewBag.PromedioGeneral = promedioGeneral;

                var viewModel = new EstudianteViewModel
                {
                    Id = estudiante.Id,
                    Nombre = estudiante.Nombre,
                    Apellido = estudiante.Apellido,
                    Matricula = estudiante.Matricula,
                    Email = estudiante.Email,
                    FechaNacimiento = estudiante.FechaNacimiento
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles del estudiante");
                TempData["Error"] = "Ocurrió un error al cargar los detalles del estudiante.";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstudianteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var estudiante = new Estudiante
                    {
                        Nombre = viewModel.Nombre,
                        Apellido = viewModel.Apellido,
                        Matricula = viewModel.Matricula,
                        Email = viewModel.Email,
                        FechaNacimiento = viewModel.FechaNacimiento
                    };

                    await _estudianteService.CreateEstudianteAsync(estudiante);
                    TempData["Success"] = "Estudiante creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear el estudiante");
                    ModelState.AddModelError("", "Ocurrió un error al crear el estudiante.");
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var estudiante = await _estudianteService.GetEstudianteByIdAsync(id);
                if (estudiante == null)
                {
                    return NotFound();
                }

                var viewModel = new EstudianteViewModel
                {
                    Id = estudiante.Id,
                    Nombre = estudiante.Nombre,
                    Apellido = estudiante.Apellido,
                    Matricula = estudiante.Matricula,
                    Email = estudiante.Email,
                    FechaNacimiento = estudiante.FechaNacimiento
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el estudiante para editar");
                TempData["Error"] = "Ocurrió un error al cargar el estudiante.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EstudianteViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var estudiante = await _estudianteService.GetEstudianteByIdAsync(id);
                    if (estudiante == null)
                    {
                        return NotFound();
                    }

                    estudiante.Nombre = viewModel.Nombre;
                    estudiante.Apellido = viewModel.Apellido;
                    estudiante.Matricula = viewModel.Matricula;
                    estudiante.Email = viewModel.Email;
                    estudiante.FechaNacimiento = viewModel.FechaNacimiento;

                    await _estudianteService.UpdateEstudianteAsync(estudiante);
                    TempData["Success"] = "Estudiante actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar el estudiante");
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el estudiante.");
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var estudiante = await _estudianteService.GetEstudianteByIdAsync(id);
                if (estudiante == null)
                {
                    return NotFound();
                }

                var viewModel = new EstudianteViewModel
                {
                    Id = estudiante.Id,
                    Nombre = estudiante.Nombre,
                    Apellido = estudiante.Apellido,
                    Matricula = estudiante.Matricula,
                    Email = estudiante.Email,
                    FechaNacimiento = estudiante.FechaNacimiento
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el estudiante para eliminar");
                TempData["Error"] = "Ocurrió un error al cargar el estudiante.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _estudianteService.DeleteEstudianteAsync(id);
                TempData["Success"] = "Estudiante eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el estudiante");
                TempData["Error"] = "Ocurrió un error al eliminar el estudiante.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}