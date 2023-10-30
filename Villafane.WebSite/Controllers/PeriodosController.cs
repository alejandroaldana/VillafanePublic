using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Mvc;
using Villafane.Domain.Models.DB;
using Villafane.Services.DB;

namespace Villafane.WebSite.Controllers
{
    public class PeriodosController : Controller
    {
        private readonly ConfigurationBusinessManager _configurationBusinessManager;

        public PeriodosController(ConfigurationBusinessManager configurationBusinessManager)
        {
            _configurationBusinessManager = configurationBusinessManager;
        }
        public async Task<IActionResult> Index()
        {
            var periodos = await _configurationBusinessManager.ObtenerPeriodosDTO();
            return View(periodos);
        }
        public async Task<IActionResult> Agregar(Periodo model)
        {
            await _configurationBusinessManager.GuardarPeriodo(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Modificar(Periodo model)
        {
            await _configurationBusinessManager.EditarPeriodo(model.Id,model.PeriodoDeEjecucion, model.Orden);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            await _configurationBusinessManager.EliminarPeriodo(id);

            return RedirectToAction("Index");
        }

    }
}
