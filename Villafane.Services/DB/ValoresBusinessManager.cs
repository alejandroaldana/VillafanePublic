using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;

namespace Villafane.Services.DB
{
    public class ValoresBusinessManager
    {
        private readonly VillafaneContext _context;

        public ValoresBusinessManager(VillafaneContext context)
        {
            _context = context;
        }

        public async Task<List<Valore>> ObtenerTodosAsync()
        {
            var elementos = await _context.Valores.ToListAsync();

            return elementos;
        }

        public async Task Agregar(Valore elemento)
        {
            if (elemento.Valor != null)
            {
                _context.Add(elemento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Editar(Valore elemento)
        {
            if (elemento != null && elemento.Id != 0)
            {
                _context.Update(elemento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Eliminar(Valore elemento)
        {
            if (elemento != null && elemento.Id != 0)
            {
                _context.Remove(elemento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PesosValoresTeorico>> ObtenerValoresTeoricos(string? periodo)
        {
            if (periodo == null)
            {
                var per = await _context.Periodos.OrderBy(x => x.Orden).FirstOrDefaultAsync();
                if (per != null)
                {
                    var elementosPrimerPeriodo = await _context.PesosValoresTeoricos.Include(x => x.IdGrupoInteresNavigation).Include(x => x.IdValorNavigation).Where(x => x.Periodo == per.PeriodoDeEjecucion).ToListAsync();
                    return elementosPrimerPeriodo;
                }
                else
                {
                    var elementos = await _context.PesosValoresTeoricos.Include(x => x.IdGrupoInteresNavigation).Include(x => x.IdValorNavigation).ToListAsync();

                    return elementos;
                }
            }
            var elementosFinales = await _context.PesosValoresTeoricos.Include(x => x.IdGrupoInteresNavigation).Include(x => x.IdValorNavigation).Where(x => x.Periodo == periodo).ToListAsync();

            return elementosFinales;
            
        }
        public async Task CalcularDisponibilidad(string periodo)
        {
            var pesos = await _context.PesosValoresTeoricos.Include(x => x.IdValorNavigation).ThenInclude(x=>x.Variables).ThenInclude(x => x.Indicadores).Where(x=>x.Periodo == periodo).ToListAsync();

            foreach (var peso in pesos)
            {
                bool encontrada = false;
                foreach (var variable in peso.IdValorNavigation.Variables)
                {
                    if (variable.Indicadores != null && variable.Indicadores.Count > 0)
                    {
                        var indicadoresGrupoInteres = variable.Indicadores.Where(x => x.IdGrupoInteres == peso.IdGrupoInteres && x.Periodo == periodo).ToList();
                        if (indicadoresGrupoInteres != null && indicadoresGrupoInteres.Count > 0)
                        {
                            peso.InformacionDisponible = true;
                            encontrada = true;
                            break;
                        }
                        
                    }
                    
                }
                if (!encontrada)
                {
                    peso.InformacionDisponible = false;
                }
                

                _context.Update(peso);
                await _context.SaveChangesAsync();
            }
        }
        public async Task GuardarValoresTeoricos(List<PesosValoresTeorico> pesos,string periodo)
        {
            if(pesos != null && pesos.Count() > 0)
            {
                foreach (var peso in pesos)
                {
                    var pesoDB = await _context.PesosValoresTeoricos
                        .FirstOrDefaultAsync(x=>x.IdValor == peso.IdValor && x.IdGrupoInteres == peso.IdGrupoInteres && x.Periodo == periodo);
                    if(pesoDB == null)
                    {
                        peso.Periodo = periodo;
                        _context.Add(peso);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        pesoDB.ValorTeorico = peso.ValorTeorico;
                        pesoDB.Tb21= peso.Tb21;
                        pesoDB.Tb22= peso.Tb22;
                        pesoDB.Tb23= peso.Tb23;
                        pesoDB.Tb3= peso.Tb3;
                        pesoDB.Tb4= peso.Tb4;
                        pesoDB.Periodo = periodo;
                        _context.Update(pesoDB);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task GuardarInformacionDisponible(List<PesosValoresTeorico> pesos)
        {
            if (pesos != null && pesos.Count() > 0)
            {
                foreach (var peso in pesos)
                {
                    var pesoDB = await _context.PesosValoresTeoricos
                        .FirstOrDefaultAsync(x => x.IdValor == peso.IdValor && x.IdGrupoInteres == peso.IdGrupoInteres);
                    if (pesoDB == null)
                    {
                        peso.ValorTeorico = 0;
                        _context.Add(peso);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        pesoDB.InformacionDisponible = peso.InformacionDisponible;
                        _context.Update(pesoDB);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        
    }
}
