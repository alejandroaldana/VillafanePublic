using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Villafane.Domain.Models.DB;
using Villafane.Domain.ViewModels;
using Villafane.Services.DB;
using Villafane.WebSite.Models;
using Villafane.WebSite.Utils;

namespace Villafane.WebSite.Controllers
{
    [Authorize]
    public class VariablesController : Controller
    {
        private readonly VariablesBusinessManager _manager;
        private readonly GrupoDeInteresBusinessManager _gdi_manager;
        private readonly ValoresBusinessManager _valores_manager;
        public VariablesController(VariablesBusinessManager manager, GrupoDeInteresBusinessManager grupoDeInteresBusinessManager, ValoresBusinessManager valoresBusinessManager)
        {
            _manager = manager;
            _gdi_manager = grupoDeInteresBusinessManager;
            _valores_manager= valoresBusinessManager;
        }   

        public async Task<IActionResult> Index()
        {
            var valores = await _manager.ObtenerTodosAsync();
            return View(valores);
        }

        public async Task<IActionResult> Agregar(Variable model)
        {
            await _manager.Agregar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Modificar(Variable model)
        {
            await _manager.Editar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(Variable model)
        {
            await _manager.Eliminar(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PesosVariablesTeoricos(string? periodo)
        {
            List<PesosVariablesTeorico> modelData = await _manager.ObtenerVariablesTeoricos(periodo);
            List<PesosValoresDefinitivo> modelDataDefinitivo = await _manager.ObtenerVariablesDefinitivo();
            ViewBag.Variables = await _manager.ObtenerTodosAsync();
            ViewBag.Gdi = await _gdi_manager.ObtenerTodosAsync();
            ViewBag.Valores = await _valores_manager.ObtenerTodosAsync();
            ViewBag.PesosValoresTeoricos = await _valores_manager.ObtenerValoresTeoricos(periodo);
            PesosVariablesTeoricosViewModel model = new PesosVariablesTeoricosViewModel();
            List<PesosVariablesTeoricoViewModel> modelB = new List<PesosVariablesTeoricoViewModel>();
            foreach (var item in modelData)
            {
                modelB.Add(new PesosVariablesTeoricoViewModel()
                {
                    IdGrupoInteres = item.IdGrupoInteres,
                    IdVariable = item.IdVariable,
                    ValorTeorico = item.ValorTeorico,
                    InformacionDisponible = item.InformacionDisponible,
                    Tb12 = item.Tb12,
                    Tb21 = item.Tb21,
                    Tb22 = item.Tb22,
                    Tb23 = item.Tb23,
                    Tb3 = item.Tb3,
                    Tb4 = item.Tb4,
                    Periodo = item.Periodo
                });
            }
            model.Pesos = modelData;
            //model.PesosDefinitivos = modelDataDefinitivo;
            ViewBag.Periodo = periodo;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PesosVariablesTeoricos(PesosVariablesTeoricosViewModel result)
        {
            await _manager.GuardarVariablesTeoricos(result.Pesos,result.Periodo);
            return RedirectToAction("PesosVariablesTeoricos");
        }


      
        [HttpPost]
        public async Task<IActionResult> GuardarValoresDefinitivos(PesosVariablesTeoricosViewModel model)
        {
            //await _manager.GuardarVariablesDefinitivos(model.PesosDefinitivos);
            return RedirectToAction("PesosVariablesTeoricos");
        }

        public async Task<IActionResult> CalcularDisponibilidad(string periodo)
        {
            await _manager.CalcularDisponibilidad(periodo);

            return RedirectToAction("PesosVariablesTeoricos");
        }

        public async Task<IActionResult> ObtenerVariablesJson()
        {
            var modelData = await _manager.ObtenerTodosAsync();

            var resultado = Mapping.MappingVariables.VariablesAViewModel(modelData);

            return Json(resultado);
        }


        public IActionResult CargarPesosVariables()
        {
            return View();
        }

        public async Task<IActionResult> DescargarArchivoCarga()
        {
            var model = await _manager.ObtenerModeloParaImportarPesosVariables();
            var dt = DataTableUtils.ConvertToDataTable(model);

            using (XLWorkbook wb = new XLWorkbook())
            {
                
                
                wb.Worksheets.Add("Pesos variables Teóricos");
                var ws = wb.Worksheets.First();
                ws.Cell(1, 1).InsertTable(dt);

                ws.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ArchivoCargaPesosVariables.xlsx");
                }
            }
        }
        [HttpPost]
        public IActionResult ImportarExcelPesosVariables(IFormFile archivo)
        {
            if (archivo != null)
            {
                if (archivo.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        archivo.CopyTo(ms);
                        _manager.CargarPesosTeoricosDeExcel(ms);

                        //_manager.CargarIndicadoresDeExcel(ms);
                        // act on the Base64 data
                    }
                }
            }

            return RedirectToAction("CargarPesosVariables");
        }
        public async Task<IActionResult> ObtenerSumaAtributos(string periodo)
        {
            var resultado = await _manager.ObtenerSumaAtributos(periodo);
            List<List<VariablesDisponiblesEnValor>> resultadoLista = new List<List<VariablesDisponiblesEnValor>>();
            for (int i = 0; i < resultado.GetLength(0); i++)
            {
                List<VariablesDisponiblesEnValor> innerList = new List<VariablesDisponiblesEnValor>();
                for (int j = 0; j < resultado.GetLength(1); j++)
                {
                    innerList.Add(resultado[i, j]);
                }
                resultadoLista.Add(innerList);
            }
            return Json(resultadoLista);
        }
    }
}
