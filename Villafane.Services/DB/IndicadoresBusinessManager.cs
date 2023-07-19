using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;
using Villafane.Domain.ViewModels;

namespace Villafane.Services.DB
{
    public class IndicadoresBusinessManager
    {
        private readonly VillafaneContext _context;

        public IndicadoresBusinessManager(VillafaneContext context)
        {
            _context = context;
        }

        public async Task<List<Indicadore>> ObtenerTodosAsync(string? periodo)
        {
            if (periodo == null)
            {
                var per = await _context.Periodos.OrderBy(x => x.Orden).FirstOrDefaultAsync();
                if (per != null)
                {
                    var elementosPrimerPeriodo = await _context.Indicadores
                .Include(x => x.IdVariableNavigation)
                .ThenInclude(x => x.IdValorNavigation)
                .Include(x => x.IdGrupoInteresNavigation)
                .Include(x => x.IdEscalaNavigation).Where(x => x.Periodo == per.PeriodoDeEjecucion).ToListAsync();
                    return elementosPrimerPeriodo;
                }
                else
                {
                    var elementos = await _context.Indicadores
                .Include(x => x.IdVariableNavigation)
                .ThenInclude(x => x.IdValorNavigation)
                .Include(x => x.IdGrupoInteresNavigation)
                .Include(x => x.IdEscalaNavigation).ToListAsync();

                    return elementos;
                }
            }
            var elementosFinales = await _context.Indicadores
                .Include(x => x.IdVariableNavigation)
                .ThenInclude(x => x.IdValorNavigation)
                .Include(x => x.IdGrupoInteresNavigation)
                .Include(x => x.IdEscalaNavigation).Where(x => x.Periodo == periodo).ToListAsync();

            
            
            return elementosFinales;
        }

        //public async Task<decimal> ObtenerNotaDeLaVariable(Indicadore indicador)
        //{
        //    var indicadores = await _context.Indicadores
        //        .Include(x => x.IdVariableNavigation)
        //        .ThenInclude(x => x.IdValorNavigation)
        //        .Include(x => x.IdGrupoInteresNavigation)
        //        .Include(x => x.IdEscalaNavigation)
        //        .Where(x=>x.IdVariable == indicador.IdVariable && x.IdGrupoInteres== x.IdGrupoInteres)
        //        .ToListAsync();
        //    if(indicadores != null && indicadores.Count> 0)
        //    {
        //        var pesoEnVariable = indicador/indicadores.Count
        //    }
            
        //}

        public async Task Agregar(Indicadore indicador)
        {
            
                _context.Add(indicador);
                await _context.SaveChangesAsync();
            
        }
        public async Task<int> AgregarEscala(Escala escala)
        {

            _context.Add(escala);
            await _context.SaveChangesAsync();

            return escala.Id;
        }

        public async Task Editar(Indicadore indicador)
        {
            if(indicador != null && indicador.Id != 0)
            {
                _context.Update(indicador);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Eliminar(Indicadore indicador)
        {
            if (indicador != null && indicador.Id != 0)
            {
                _context.Remove(indicador);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<Escala>> ObtenerEscalasAsync()
        {
            var escalas = await _context.Escalas.ToListAsync();

            return escalas;
        }


        public  bool CargarIndicadoresDeExcel(Stream archivo)
        {
            using (XLWorkbook workBook = new XLWorkbook(archivo))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet("Indicadores");



                //Create a new DataTable.
                DataTable dt = new DataTable();

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

                List<IndicadoresExcelViewModel> indicadoresExcel = dt.AsEnumerable().Select(m => new IndicadoresExcelViewModel()
                {
                    GDI = m.Field<string>("IDGDI"),
                    VARIABLE = m.Field<string>("IDVARIABLE"),
                    NOMBRE_INDICADOR = m.Field<string>("NOMBREINDICADOR"),
                    FUENTE = m.Field<string>("FUENTE"),
                    AÑO = m.Field<string>("AÑO"),
                    AÑO_USO = m.Field<string>("AÑOUSO"),
                    ESCALA = m.Field<string>("ESCALA"),
                    MIN_ESCALA = m.Field<string>("MINESCALA"),
                    MAX_ESCALA = m.Field<string>("MAXESCALA"),
                    DATO = m.Field<string>("DATO"),
                    DATO_NORMALIZADO = m.Field<string>("DATONORMALIZADO"),
                    PERIODO = m.Field<string>("PERIODO")
                }).ToList();

                NumberFormatInfo numberFormatWithComma = new NumberFormatInfo();
                numberFormatWithComma.NumberDecimalSeparator = ",";
                foreach (var indExcel in indicadoresExcel)
                {
                    Indicadore indicador = new Indicadore();

                    if (!string.IsNullOrEmpty(indExcel.ESCALA))
                    {
                        Escala? escala = _context.Escalas.FirstOrDefault(x=>x.Descripcion == indExcel.ESCALA);

                        if(escala != null)
                        {
                            indicador.IdEscala = escala.Id;
                        }
                        else
                        {
                            escala = new Escala();
                            escala.Descripcion = indExcel.ESCALA;
                            escala.Min = decimal.Parse(indExcel.MIN_ESCALA, numberFormatWithComma);
                            escala.Max = decimal.Parse(indExcel.MAX_ESCALA, numberFormatWithComma);
                            _context.Add(escala);
                            _context.SaveChanges();
                            indicador.IdEscala = escala.Id;
                        }
                       
                    }
                    if (!string.IsNullOrEmpty(indExcel.GDI))
                    {
                        int idgdi = int.Parse(indExcel.GDI);
                        GruposDeIntere? gdi = _context.GruposDeInteres.FirstOrDefault(x=>x.Id== idgdi);

                        if(gdi != null)
                        {
                            indicador.IdGrupoInteres= gdi.Id;
                        }
                    }
                    if (!string.IsNullOrEmpty(indExcel.VARIABLE))
                    {
                        int idvariable = int.Parse(indExcel.VARIABLE);
                        Variable? variable = _context.Variables.FirstOrDefault(x => x.Id == idvariable);

                        if (variable != null)
                        {
                            indicador.IdVariable = variable.Id;
                        }
                    }
                    indicador.Indicador = indExcel.NOMBRE_INDICADOR;
                    indicador.Fuente = indExcel.FUENTE;
                    indicador.FechaIndicador = int.Parse(indExcel.AÑO);
                    indicador.FechaUsoIndicador= int.Parse(indExcel.AÑO_USO);
                    indicador.Dato = decimal.Parse(indExcel.DATO, numberFormatWithComma);
                    indicador.DatoNormalizado  = decimal.Parse(indExcel.DATO_NORMALIZADO, numberFormatWithComma);
                    indicador.Periodo = indExcel.PERIODO;
                    _context.Add(indicador);
                    _context.SaveChanges();
                }


            }


            return true;
        }

        public async Task<(List<DescargarExcelIndicadoresViewModel>,List<DescargarExcelListadoGDI>,List<DescargarExcelListadoVariables>)> ObtenerModeloParaImportarIndicadores()
        {
            var gdis = await _context.GruposDeInteres.ToListAsync();
            var variables = await _context.Variables.Include(x=>x.IdValorNavigation).ToListAsync();
            List<DescargarExcelListadoGDI> modeloGDI = new List<DescargarExcelListadoGDI>();
            foreach (var gdi in gdis)
            {
                modeloGDI.Add(new DescargarExcelListadoGDI()
                {
                    IDGDI = gdi.Id,
                    GDI = gdi.Grupo
                });
            }
            List<DescargarExcelListadoVariables> modeloVariables = new List<DescargarExcelListadoVariables>();
            foreach (var variable in variables)
            {
                modeloVariables.Add(new DescargarExcelListadoVariables()
                {
                    IDVARIABLE = variable.Id,
                    VALOR = variable.IdValorNavigation.Valor,
                    VARIABLE = variable.Variable1
                });
            }
            List<DescargarExcelIndicadoresViewModel> modeloIndicadores = new List<DescargarExcelIndicadoresViewModel>();
            return (modeloIndicadores,modeloGDI,modeloVariables);
        }
    }
}
