using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Villafane.Domain.Models.DB;
using Villafane.Services.DB;

namespace Villafane.WebSite.Controllers
{
    [Authorize]
    public class GruposDeInteresController : Controller
    {
        private readonly GrupoDeInteresBusinessManager _manager;

        public GruposDeInteresController(GrupoDeInteresBusinessManager manager)
        {
            _manager = manager;
        }   

        public async Task<IActionResult> Index()
        {
            List<GruposDeIntere> gruposDeInteres = await _manager.ObtenerTodosAsync();
            return View(gruposDeInteres);
        }

        public async Task<IActionResult> Agregar(GruposDeIntere model)
        {
            await _manager.Agregar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Modificar(GruposDeIntere model)
        {
            await _manager.Editar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(GruposDeIntere model)
        {
            await _manager.Eliminar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ObtenerGruposDeInteresJson()
        {
            var result = await _manager.ObtenerTodosAsync();
            return Json(result);
        }
    }
}
