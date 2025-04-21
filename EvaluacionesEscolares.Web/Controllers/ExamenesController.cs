using EvaluacionesEscolares.Core.Services;
using EvaluacionesEscolares.Data;
using EvaluacionesEscolares.Entities;
using EvaluacionesEscolares.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionesEscolares.Web.Controllers
{
    public class ExamenesController : Controller
    {
        private readonly IExamenService _examenService;
        private readonly ICalificacionService _calificacionService;
        private readonly ILogger<ExamenesController> _logger;
        private readonly ApplicationDbContext _context;

        public ExamenesController(
            IExamenService examenService,
            ICalificacionService calificacionService,
            ApplicationDbContext context,
            ILogger<ExamenesController> logger)
        {
            _examenService = examenService;
            _calificacionService = calificacionService;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var examenes = await _examenService.GetExamenesCompletosAsync();
                var viewModels = examenes.Select(e => new ExamenViewModel
                {
                    Id = e.Id,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    Duracion = e.Duracion,
                    MateriaId = e.MateriaId,
                    MateriaNombre = e.Materia?.Nombre ?? "Sin materia",
                    FechaCreacion = e.FechaCreacion
                }).ToList();

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de exámenes");
                TempData["Error"] = "Ocurrió un error al cargar los exámenes.";
                return View(new List<ExamenViewModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var examen = await _examenService.GetExamenCompletoAsync(id);
                if (examen == null)
                {
                    return NotFound();
                }

                var viewModel = new ExamenViewModel
                {
                    Id = examen.Id,
                    Titulo = examen.Titulo,
                    Descripcion = examen.Descripcion,
                    Duracion = examen.Duracion,
                    MateriaId = examen.MateriaId,
                    MateriaNombre = examen.Materia?.Nombre ?? "Sin materia",
                    FechaCreacion = examen.FechaCreacion,
                    Preguntas = examen.Preguntas?.Select(p => new PreguntaViewModel
                    {
                        Id = p.Id,
                        TextoPregunta = p.TextoPregunta,
                        Puntos = p.Puntos,
                        ExamenId = p.ExamenId,
                        Opciones = p.Opciones?.Select(o => new OpcionViewModel
                        {
                            Id = o.Id,
                            TextoOpcion = o.TextoOpcion,
                            EsCorrecta = o.EsCorrecta,
                            PreguntaId = o.PreguntaId
                        }).ToList() ?? new List<OpcionViewModel>()
                    }).ToList() ?? new List<PreguntaViewModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles del examen");
                TempData["Error"] = "Ocurrió un error al cargar los detalles del examen.";
                return RedirectToAction(nameof(Index));
            }
        }

        // MÉTODO MODIFICADO: Create (GET)
        public async Task<IActionResult> Create()
        {
            try
            {
                // Verificar si hay materias disponibles
                var materias = await _context.Materias.ToListAsync();

                if (materias == null || !materias.Any())
                {
                    TempData["Warning"] = "No hay materias disponibles. Por favor, cree algunas materias primero.";
                    return RedirectToAction("Index", "Materias");
                }

                ViewBag.Materias = new SelectList(materias, "Id", "Nombre");
                _logger.LogInformation("Cargadas {Count} materias para el formulario de creación de examen", materias.Count);

                return View(new ExamenViewModel { FechaCreacion = DateTime.Now });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar las materias para el formulario de examen");
                TempData["Error"] = "Ocurrió un error al cargar las materias: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // MÉTODO MODIFICADO: Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamenViewModel viewModel)
        {
            _logger.LogInformation("Intentando crear examen: {Titulo}, MateriaId: {MateriaId}",
                viewModel.Titulo, viewModel.MateriaId);

            // Verificar si el modelo es válido
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido al crear examen");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Error en {Field}: {Error}", state.Key, error.ErrorMessage);
                    }
                }

                try
                {
                    var materias = await _context.Materias.ToListAsync();
                    ViewBag.Materias = new SelectList(materias, "Id", "Nombre", viewModel.MateriaId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al recargar las materias después de validación fallida");
                    ViewBag.Materias = new SelectList(new List<Materia>(), "Id", "Nombre");
                }

                return View(viewModel);
            }

            try
            {
                // Verificar si la materia existe
                var materiaExiste = await _context.Materias.AnyAsync(m => m.Id == viewModel.MateriaId);
                if (!materiaExiste)
                {
                    _logger.LogWarning("Intento de crear examen con MateriaId inexistente: {MateriaId}", viewModel.MateriaId);
                    ModelState.AddModelError("MateriaId", "La materia seleccionada no existe.");

                    var materias = await _context.Materias.ToListAsync();
                    ViewBag.Materias = new SelectList(materias, "Id", "Nombre");

                    return View(viewModel);
                }

                var examen = new Examen
                {
                    Titulo = viewModel.Titulo,
                    Descripcion = viewModel.Descripcion,
                    Duracion = viewModel.Duracion,
                    MateriaId = viewModel.MateriaId,
                    FechaCreacion = DateTime.Now
                };

                _logger.LogInformation("Creando examen en la base de datos");
                await _examenService.CreateExamenAsync(examen);

                _logger.LogInformation("Examen creado correctamente con Id: {Id}", examen.Id);
                TempData["Success"] = "Examen creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el examen: {Message}", ex.Message);
                TempData["Error"] = "Ocurrió un error al crear el examen: " + ex.Message;

                try
                {
                    var materias = await _context.Materias.ToListAsync();
                    ViewBag.Materias = new SelectList(materias, "Id", "Nombre", viewModel.MateriaId);
                }
                catch
                {
                    ViewBag.Materias = new SelectList(new List<Materia>(), "Id", "Nombre");
                }

                return View(viewModel);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var examen = await _examenService.GetExamenCompletoAsync(id);
                if (examen == null)
                {
                    return NotFound();
                }

                var viewModel = new ExamenViewModel
                {
                    Id = examen.Id,
                    Titulo = examen.Titulo,
                    Descripcion = examen.Descripcion,
                    Duracion = examen.Duracion,
                    MateriaId = examen.MateriaId,
                    FechaCreacion = examen.FechaCreacion,
                    Preguntas = examen.Preguntas?.Select(p => new PreguntaViewModel
                    {
                        Id = p.Id,
                        TextoPregunta = p.TextoPregunta,
                        Puntos = p.Puntos,
                        ExamenId = p.ExamenId,
                        Opciones = p.Opciones?.Select(o => new OpcionViewModel
                        {
                            Id = o.Id,
                            TextoOpcion = o.TextoOpcion,
                            EsCorrecta = o.EsCorrecta,
                            PreguntaId = o.PreguntaId
                        }).ToList() ?? new List<OpcionViewModel>()
                    }).ToList() ?? new List<PreguntaViewModel>()
                };

                ViewBag.Materias = new SelectList(_context.Materias, "Id", "Nombre", viewModel.MateriaId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el examen para editar");
                TempData["Error"] = "Ocurrió un error al cargar el examen.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExamenViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var examen = await _examenService.GetExamenByIdAsync(id);
                    if (examen == null)
                    {
                        return NotFound();
                    }

                    examen.Titulo = viewModel.Titulo;
                    examen.Descripcion = viewModel.Descripcion;
                    examen.Duracion = viewModel.Duracion;
                    examen.MateriaId = viewModel.MateriaId;

                    await _examenService.UpdateExamenAsync(examen);
                    TempData["Success"] = "Examen actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar el examen");
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el examen.");
                }
            }

            ViewBag.Materias = new SelectList(_context.Materias, "Id", "Nombre", viewModel.MateriaId);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var examen = await _examenService.GetExamenByIdAsync(id);
                if (examen == null)
                {
                    return NotFound();
                }

                var viewModel = new ExamenViewModel
                {
                    Id = examen.Id,
                    Titulo = examen.Titulo,
                    Descripcion = examen.Descripcion,
                    Duracion = examen.Duracion,
                    MateriaId = examen.MateriaId,
                    MateriaNombre = examen.Materia?.Nombre ?? "Sin materia",
                    FechaCreacion = examen.FechaCreacion
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el examen para eliminar");
                TempData["Error"] = "Ocurrió un error al cargar el examen.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _examenService.DeleteExamenAsync(id);
                TempData["Success"] = "Examen eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el examen");
                TempData["Error"] = "Ocurrió un error al eliminar el examen.";
                return RedirectToAction(nameof(Index));
            }
        }

        // MÉTODO NUEVO: Debug (para diagnóstico)
        public async Task<IActionResult> Debug()
        {
            var viewModel = new DebugViewModel
            {
                MateriasCount = await _context.Materias.CountAsync(),
                Materias = await _context.Materias.ToListAsync(),
                ExamenesCount = await _context.Examenes.CountAsync(),
                ConnectionString = _context.Database.GetConnectionString()
            };

            return View(viewModel);
        }

        // MÉTODO NUEVO: TestCreateDirect (para diagnóstico de creación directa)
        public async Task<IActionResult> TestCreateDirect()
        {
            try
            {
                // Obtener la primera materia (si existe)
                var materia = await _context.Materias.FirstOrDefaultAsync();
                if (materia == null)
                {
                    TempData["Error"] = "No hay materias disponibles para crear un examen de prueba.";
                    return RedirectToAction(nameof(Index));
                }

                // Crear un examen de prueba
                var examen = new Examen
                {
                    Titulo = "Examen de prueba directo " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Descripcion = "Este es un examen de prueba creado directamente con el contexto",
                    Duracion = 60,
                    MateriaId = materia.Id,
                    FechaCreacion = DateTime.Now
                };

                // Intentar guardar directamente en la base de datos
                _context.Examenes.Add(examen);
                var result = await _context.SaveChangesAsync();

                TempData["Success"] = $"Examen de prueba creado con ID {examen.Id}. SaveChanges devolvió {result}.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TestCreateDirect: {Message}", ex.Message);
                TempData["Error"] = $"Error al crear examen de prueba directo: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }

    // CLASE NUEVA: DebugViewModel (para la página de depuración)
    public class DebugViewModel
    {
        public int MateriasCount { get; set; }
        public List<Materia> Materias { get; set; }
        public int ExamenesCount { get; set; }
        public string ConnectionString { get; set; }
    }
}