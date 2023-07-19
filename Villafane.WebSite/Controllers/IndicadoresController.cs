using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Villafane.Domain.Models.DB;
using Villafane.Services.DB;
using Villafane.WebSite.Utils;

namespace Villafane.WebSite.Controllers
{
    [Authorize]
    public class IndicadoresController : Controller
    {
        private readonly IndicadoresBusinessManager _manager;
        private readonly VariablesBusinessManager _var_manager;
        public IndicadoresController(IndicadoresBusinessManager manager, VariablesBusinessManager variablesBusinessManager)
        {
            _manager = manager;
            _var_manager = variablesBusinessManager;
        }   

        public async Task<IActionResult> Index(string? periodo)
        {
            List<Indicadore> indicadores = await _manager.ObtenerTodosAsync(periodo);
            ViewBag.Periodo = periodo;
            return View(indicadores);
        }

        public async Task<IActionResult> Agregar(Indicadore model)
        {
            await _manager.Agregar(model);
            await _var_manager.CalcularDisponibilidad(model.Periodo);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Modificar(Indicadore model)
        {
            await _manager.Editar(model);
            await _var_manager.CalcularDisponibilidad(model.Periodo);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(Indicadore model)
        {
            await _manager.Eliminar(model);
            await _var_manager.CalcularDisponibilidad(model.Periodo);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AgregarEscala(string descripcion, decimal min, decimal max)
        {
            Escala escala = new Escala()
            {
                Descripcion = descripcion,
                Min = min,
                Max = max
            };
            int idEscala = await _manager.AgregarEscala(escala);

            return Json(idEscala);
        }

        public IActionResult ImportarExcelIndicadores()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportarExcelIndicadores(IFormFile archivo)
        {
            if(archivo != null)
            {
                if (archivo.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        archivo.CopyTo(ms);
                       
                        
                        _manager.CargarIndicadoresDeExcel(ms);
                        // act on the Base64 data
                    }
                }
            }

            return View();
        }

        public async Task<IActionResult> IR(string? periodo)
        {
            List<Indicadore> indicadores = await _manager.ObtenerTodosAsync(periodo);
            return View(indicadores);
        }

        public async Task<IActionResult> GenerarExcelImportacionIndicadores()
        {
            var model = await _manager.ObtenerModeloParaImportarIndicadores();
            var dtIndicadores = DataTableUtils.ConvertToDataTable(model.Item1);
            var dtGDI = DataTableUtils.ConvertToDataTable(model.Item2);
            var dtVariables = DataTableUtils.ConvertToDataTable(model.Item3);

            using (XLWorkbook wb = new XLWorkbook())
            {


                wb.Worksheets.Add("Indicadores");
                var ws = wb.Worksheets.First();
                ws.Cell(1, 1).InsertTable(dtIndicadores);

                ws.Columns().AdjustToContents();

                wb.Worksheets.Add("Variables");
                var wsVariables = wb.Worksheets.FirstOrDefault(x => x.Name == "Variables");
                wsVariables.Cell(1, 1).InsertTable(dtVariables);

                wsVariables.Columns().AdjustToContents();

                wb.Worksheets.Add("GDI");
                var wsGDI = wb.Worksheets.FirstOrDefault(x => x.Name == "GDI");
                wsGDI.Cell(1, 1).InsertTable(dtGDI);

                wsVariables.Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ArchivoCargaIndicadores.xlsx");
                }
            }
        }
    }
}
