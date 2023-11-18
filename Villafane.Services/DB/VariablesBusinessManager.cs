using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;
using Villafane.Domain.ViewModels;

namespace Villafane.Services.DB
{
    public class VariablesBusinessManager
    {
        private readonly VillafaneContext _context;

        public VariablesBusinessManager(VillafaneContext context)
        {
            _context = context;
        }

        public async Task<List<Variable>> ObtenerTodosAsync()
        {
            var elementos = await _context.Variables.Include(x => x.IdValorNavigation).ToListAsync();

            return elementos;
        }

        public async Task Agregar(Variable elemento)
        {
            if (elemento.Variable1 != null && elemento.IdValor != 0)
            {
                _context.Add(elemento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Editar(Variable elemento)
        {
            if (elemento != null && elemento.Id != 0 && elemento.IdValor != 0)
            {
                _context.Update(elemento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Eliminar(Variable elemento)
        {
            if (elemento != null && elemento.Id != 0)
            {
                _context.Remove(elemento);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<PesosVariablesTeorico>> ObtenerVariablesTeoricos(string? periodo)
        {
            if (periodo == null)
            {
                var per = await _context.Periodos.OrderBy(x => x.Orden).FirstOrDefaultAsync();
                if (per != null)
                {
                    var elementosPrimerPeriodo = await _context.PesosVariablesTeoricos.Where(x => x.Periodo == per.PeriodoDeEjecucion).ToListAsync();
                    return elementosPrimerPeriodo;
                }
                else
                {
                    var elementos = await _context.PesosVariablesTeoricos.ToListAsync();

                    return elementos;
                }
            }
            var elementosFinales = await _context.PesosVariablesTeoricos.Where(x => x.Periodo == periodo).ToListAsync();

            return elementosFinales;
        }

        public async Task<List<PesosValoresDefinitivo>> ObtenerVariablesDefinitivo()
        {
            var elementos = await _context.PesosValoresDefinitivos.ToListAsync();

            return elementos;
        }
        public async Task GuardarVariablesTeoricos(List<PesosVariablesTeorico> pesos, string periodo)
        {
            if (pesos != null && pesos.Count() > 0)
            {
                foreach (var peso in pesos)
                {
                    var pesoDB = await _context.PesosVariablesTeoricos
                        .FirstOrDefaultAsync(x => x.IdVariable == peso.IdVariable && x.IdGrupoInteres == peso.IdGrupoInteres && x.Periodo == periodo);
                    if (pesoDB == null)
                    {
                        peso.Periodo = periodo;
                        _context.Add(peso);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        pesoDB.ValorTeorico = peso.ValorTeorico;
                        pesoDB.Tb12 = peso.Tb12;
                        pesoDB.Tb21 = peso.Tb21;
                        pesoDB.Tb22 = peso.Tb22;
                        pesoDB.Tb23 = peso.Tb23;
                        pesoDB.Tb3 = peso.Tb3;
                        pesoDB.Tb4 = peso.Tb4;
                        pesoDB.Periodo = periodo;
                        _context.Update(pesoDB);
                        await _context.SaveChangesAsync();
                    }
                }

                await CalcularYGuardarIR(periodo);
            }
        }


        public async Task CalcularYGuardarIR(string periodo)
        {
            var pesosVariables = await ObtenerVariablesTeoricos(periodo);
            var pesosValores = await _context.PesosValoresTeoricos.Include(x => x.IdGrupoInteresNavigation).Include(x => x.IdValorNavigation).Where(x => x.Periodo == periodo).ToListAsync();
            var indicadores = await _context.Indicadores
               .Include(x => x.IdVariableNavigation)
               .ThenInclude(x => x.IdValorNavigation)
               .Include(x => x.IdGrupoInteresNavigation)
               .Include(x => x.IdEscalaNavigation)
               .Where(x => x.Periodo == periodo)
               .ToListAsync();

            foreach (var indicador in indicadores)
            {
                var pesoVariable = pesosVariables.FirstOrDefault(x => x.IdVariable == indicador.IdVariable && x.IdGrupoInteres == indicador.IdGrupoInteres);
                if (pesoVariable != null)
                {
                    int numIndicadoresEnVariable = indicadores.Where(x => x.IdVariable == indicador.IdVariable && x.IdGrupoInteres == indicador.IdGrupoInteres && x.Periodo == periodo).Count();
                    decimal? pesoVariableEnIR = pesoVariable.Tb23;

                    if (numIndicadoresEnVariable > 0 && pesoVariableEnIR != null && pesoVariableEnIR != 0)
                    {
                        indicador.PesoIndicadorEnIr = pesoVariableEnIR / numIndicadoresEnVariable;
                        indicador.PesoIndicadorEnVariable = indicador.PesoIndicadorEnIr / pesoVariableEnIR;
                    }
                    else
                    {
                        indicador.PesoIndicadorEnIr = 0;
                        indicador.PesoIndicadorEnVariable = 0;
                    }
                    indicador.PesoIndicadorEnVariableNormalizado = indicador.PesoIndicadorEnVariable * indicador.DatoNormalizado;


                }

                var pesoValor = pesosValores.FirstOrDefault(x => x.IdValor == indicador.IdVariableNavigation.IdValor && x.IdGrupoInteres == indicador.IdGrupoInteres);
                if (pesoValor != null)
                {
                    if (pesoValor.Tb23 != null && pesoValor.Tb23 != 0)
                    {
                        indicador.PesoIndicadorEnValor = indicador.PesoIndicadorEnIr / pesoValor.Tb23;

                    }
                    else
                    {
                        indicador.PesoIndicadorEnValor = 0;

                    }

                    indicador.PesoIndicadorEnValorNormalizado = indicador.PesoIndicadorEnValor * indicador.DatoNormalizado;


                    if (pesoValor.Tb4 != null)
                    {
                        indicador.PesoDelGdienValor = pesoValor.Tb4;
                    }
                }
            }
            foreach (var indicador in indicadores)
            {

                var indicadoresSuma = indicadores.Where(x => x.IdVariable == indicador.IdVariable && x.IdGrupoInteres == indicador.IdGrupoInteres).Select(x => x.PesoIndicadorEnVariableNormalizado).Sum();
                var indicadoresValoresSuma = indicadores.Where(x => x.IdVariableNavigation.IdValorNavigation == indicador.IdVariableNavigation.IdValorNavigation && x.IdGrupoInteres == indicador.IdGrupoInteres).Select(x => x.PesoIndicadorEnValorNormalizado).Sum();
                indicador.NotaDeLaVariable = indicadoresSuma;
                indicador.NotaDelValor = indicadoresValoresSuma;
            }







            var gdis = await _context.GruposDeInteres.ToListAsync();
            foreach (var gdi in gdis)
            {
                var pesosValoresGdi = pesosValores.Where(x => x.IdGrupoInteres == gdi.Id).ToList();
                var sumaPesos = pesosValoresGdi.Select(x => x.Tb23).Sum();
                gdi.PesoEnIr = sumaPesos;
                _context.Update(gdi);
                await _context.SaveChangesAsync();
            }

            foreach (var indicador in indicadores)
            {
                var gdi = gdis.First(x => x.Id == indicador.IdGrupoInteres);
                indicador.PesoIndicadorEnGdi = gdi.PesoEnIr * indicador.PesoIndicadorEnIr;
                indicador.PesoIndicadorEnGdinormalizado = (indicador.PesoIndicadorEnGdi * indicador.DatoNormalizado) / 100;
            }


            _context.UpdateRange(indicadores);
            await _context.SaveChangesAsync();
        }


        public async Task GuardarVariablesDefinitivos(List<PesosValoresDefinitivo> pesos)
        {
            if (pesos != null && pesos.Count() > 0)
            {
                foreach (var peso in pesos)
                {
                    var pesoDB = await _context.PesosValoresDefinitivos
                        .FirstOrDefaultAsync(x => x.IdVariable == peso.IdVariable && x.IdGrupoInteres == peso.IdGrupoInteres);
                    if (pesoDB == null)
                    {
                        _context.Add(peso);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        pesoDB.ValorDefinitivo = peso.ValorDefinitivo;
                        _context.Update(pesoDB);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        public async Task CalcularDisponibilidad(string periodo)
        {
            var pesos = await _context.PesosVariablesTeoricos.Include(x => x.IdVariableNavigation).ThenInclude(x => x.Indicadores).Where(x => x.Periodo == periodo).ToListAsync();

            foreach (var peso in pesos)
            {
                if (peso.IdVariableNavigation.Indicadores != null && peso.IdVariableNavigation.Indicadores.Count > 0)
                {
                    var indicadoresGrupoInteres = peso.IdVariableNavigation.Indicadores.Where(x => x.IdGrupoInteres == peso.IdGrupoInteres && x.Periodo == periodo).ToList();
                    if (indicadoresGrupoInteres != null && indicadoresGrupoInteres.Count > 0)
                    {
                        peso.InformacionDisponible = true;
                    }
                    else
                    {
                        peso.InformacionDisponible = false;
                    }
                }
                else
                {
                    peso.InformacionDisponible = false;
                }

                _context.Update(peso);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CrearPesosBase(string periodo)
        {
            var pesos = await _context.PesosVariablesTeoricos.Where(x => x.Periodo == periodo).ToListAsync();
            if (pesos.Count == 0)
            {
                var gdis = await _context.GruposDeInteres.ToListAsync();
                var variables = await _context.Variables.ToListAsync();
                foreach (var gdi in gdis)
                {
                    foreach (var variable in variables)
                    {
                        PesosVariablesTeorico pesoDB = new PesosVariablesTeorico();
                        pesoDB.IdVariable = variable.Id;
                        pesoDB.IdGrupoInteres = gdi.Id;
                        pesoDB.ValorTeorico = 0;
                        pesoDB.Tb12 = 0;
                        pesoDB.Tb21 = 0;
                        pesoDB.Tb22 = 0;
                        pesoDB.Tb23 = 0;
                        pesoDB.Tb3 = 0;
                        pesoDB.Tb4 = 0;
                        pesoDB.Periodo = periodo;
                        _context.Add(pesoDB);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            return true;
        }

        public async Task<VariablesDisponiblesEnValor[,]> ObtenerSumaAtributos(string periodo)
        {
            await CrearPesosBase(periodo);
            await CalcularDisponibilidad(periodo);
            var gdis = await _context.GruposDeInteres.ToListAsync();
            var valores = await _context.Valores.Include(x => x.Variables).ToListAsync();
            VariablesDisponiblesEnValor[,] resultado = new VariablesDisponiblesEnValor[valores.Count(), gdis.Count()];
            int i = 0;
            int j = 0;
            foreach (var valor in valores)
            {
                i = 0;
                foreach (var gdi in gdis)
                {
                
                    var variables = await _context.Variables.Where(x => x.IdValor == valor.Id).ToListAsync();
                    var numvariables = variables.Count();
                    var numdisponible = 0;
                    foreach (var variable in variables)
                    {
                        bool vardisponible = await _context.PesosVariablesTeoricos.Where(x => x.IdVariable == variable.Id && x.IdGrupoInteres == gdi.Id && x.Periodo == periodo).Select(x => x.InformacionDisponible).FirstOrDefaultAsync();
                        if (vardisponible == true)
                        {
                            numdisponible++;
                        }
                    }
                    VariablesDisponiblesEnValor v = new VariablesDisponiblesEnValor();
                    v.numvariables = numvariables;
                    v.numvariablesdisponibles = numdisponible;
                    resultado[j, i] = v;
                    i++;
                }
                j++;
            }
            return resultado;
        }

        public bool CargarPesosTeoricosDeExcel(Stream archivo)
        {
            using (XLWorkbook workBook = new XLWorkbook(archivo))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet("Pesos variables Teóricos");



                //Create a new DataTable.
                System.Data.DataTable dt = new System.Data.DataTable();

                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    Console.WriteLine("Accessing to a new row in the excel ");
                    if (firstRow)
                    {
                        Console.WriteLine("Creating table columns");
                        //Create columns
                        foreach (IXLCell cell in row.Cells(workSheet.FirstCellUsed().Address.ColumnNumber, workSheet.LastCellUsed().Address.ColumnNumber))
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        Console.WriteLine("Adding new row to the data table");
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells(workSheet.FirstCellUsed().Address.ColumnNumber, workSheet.LastCellUsed().Address.ColumnNumber))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value;
                            i++;
                        }
                    }



                }

                List<PesosVariablesTeoricosExcelViewModel> indicadoresExcel = dt.AsEnumerable().Select(m => new PesosVariablesTeoricosExcelViewModel()
                {
                    GDI = m.Field<string>("IdGDI"),
                    VARIABLE = m.Field<string>("IdVariable"),
                    DATO = m.Field<string>("Dato"),
                    PERIODO = m.Field<string>("Periodo")
                }).ToList();

                NumberFormatInfo numberFormatWithComma = new NumberFormatInfo();
                numberFormatWithComma.NumberDecimalSeparator = ",";
                foreach (var indExcel in indicadoresExcel)
                {
                    PesosVariablesTeorico pvt = new PesosVariablesTeorico();

                    pvt.IdVariable = int.Parse(indExcel.VARIABLE);
                    pvt.IdGrupoInteres = int.Parse(indExcel.GDI);
                    pvt.ValorTeorico = decimal.Parse(indExcel.DATO, numberFormatWithComma);
                    pvt.Periodo = indExcel.PERIODO;

                    var pesoTeoricoDB = _context.PesosVariablesTeoricos.FirstOrDefault(x => x.IdVariable == pvt.IdVariable && x.IdGrupoInteres == pvt.IdGrupoInteres && x.Periodo == pvt.Periodo);
                    if (pesoTeoricoDB != null)
                    {
                        pesoTeoricoDB.ValorTeorico = pvt.ValorTeorico;
                        _context.Update(pesoTeoricoDB);
                        _context.SaveChanges();
                    }
                    else
                    {

                        _context.Add(pvt);
                        _context.SaveChanges();

                    }
                }


            }


            return true;
        }

        public async Task<List<DescargarExcelPesosVariablesViewModel>> ObtenerModeloParaImportarPesosVariables()
        {
            var gdis = await _context.GruposDeInteres.ToListAsync();
            var variables = await _context.Variables.ToListAsync();
            List<DescargarExcelPesosVariablesViewModel> modelo = new List<DescargarExcelPesosVariablesViewModel>();
            foreach (var gdi in gdis)
            {
                foreach (var variable in variables)
                {
                    modelo.Add(new DescargarExcelPesosVariablesViewModel
                    {
                        IdVariable = variable.Id,
                        Variable = variable.Variable1,
                        IdGDI = gdi.Id,
                        GDI = gdi.Grupo,
                        Dato = 0
                    });
                }
            }

            return modelo;
        }
    }
}
