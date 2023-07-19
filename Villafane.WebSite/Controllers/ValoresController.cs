using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Villafane.Domain.Models.DB;
using Villafane.Services.DB;
using Villafane.WebSite.Models;

namespace Villafane.WebSite.Controllers
{
    [Authorize]
    public class ValoresController : Controller
    {
        private readonly ValoresBusinessManager _manager;
        private readonly GrupoDeInteresBusinessManager _gdi_manager;
        public ValoresController(ValoresBusinessManager manager, GrupoDeInteresBusinessManager grupoDeInteresBusinessManager)
        {
            _manager = manager;
            _gdi_manager = grupoDeInteresBusinessManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Valore> valores = await _manager.ObtenerTodosAsync();
            return View(valores);
        }

        public async Task<IActionResult> Agregar(Valore model)
        {
            await _manager.Agregar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Modificar(Valore model)
        {
            await _manager.Editar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(Valore model)
        {
            await _manager.Eliminar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PesosValoresTeoricos(string? periodo)
        {
            List<PesosValoresTeorico> modelData = await _manager.ObtenerValoresTeoricos(periodo);
            ViewBag.Valores = await _manager.ObtenerTodosAsync();
            ViewBag.Gdi = await _gdi_manager.ObtenerTodosAsync();
            PesosValoresTeoricosViewModel model = new PesosValoresTeoricosViewModel();
            model.Pesos = modelData;
            ViewBag.Periodo = periodo;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PesosValoresTeoricos(PesosValoresTeoricosViewModel model)
        {
            await _manager.GuardarValoresTeoricos(model.Pesos, model.Periodo);
            await _manager.CalcularDisponibilidad(model.Periodo);
            return RedirectToAction("PesosValoresTeoricos");
        }

        [HttpPost]
        public async Task<IActionResult> MatrizInformacionDisponible(PesosValoresTeoricosViewModel model)
        {
            await _manager.GuardarInformacionDisponible(model.Pesos);
            return RedirectToAction("PesosValoresTeoricos");
        }

        public async Task<IActionResult> ObtenerPesosValores(string periodo)
        {
            List<PesosValoresTeorico> modelData = await _manager.ObtenerValoresTeoricos(periodo);

            var resultado = Mapping.MappingValores.PesosValoresTeoricoAViewModel(modelData);

            return Json(resultado);
        }
    }
}
