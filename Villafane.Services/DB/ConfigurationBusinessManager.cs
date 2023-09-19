using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;

namespace Villafane.Services.DB
{
    public class ConfigurationBusinessManager
    {
        private readonly VillafaneContext _context;

        public ConfigurationBusinessManager(VillafaneContext context)
        {
            _context = context;
        }

        public async Task<Configuracion?> ObtenerConfiguracionAsync()
        {
            var config = await _context.Configuracions.FirstOrDefaultAsync();

            return config;
        }
        public async Task<Configuracion?> ObtenerConfiguracionConLogoAsync()
        {
            var config = await _context.Configuracions.Include(x=>x.IdLogoNavigation).FirstOrDefaultAsync();

            return config;
        }

        public async Task ModificarConfiguracion(Configuracion config)
        {
            if(config == null)
            {
                return;
            }

            if(config.Id == 0)
            {
                if(config.IdLogoNavigation != null)
                {
                    LogotipoCliente logo = new LogotipoCliente()
                    {
                        BinarioLogo = config.IdLogoNavigation.BinarioLogo,
                        ContentTypeLogo = config.IdLogoNavigation.ContentTypeLogo,
                        ExtensionLogo = config.IdLogoNavigation.ExtensionLogo
                    };
                    await _context.AddAsync(logo);
                    await _context.SaveChangesAsync();

                    config.IdLogo = logo.Id;

                }
               
                await _context.AddAsync(config);
                await _context.SaveChangesAsync();

            }
            else
            {
                _context.Update(config);
                await _context.SaveChangesAsync();
            }
            return;
        }

       public async Task<List<string>> ObtenerPeriodos()
        {
            return await _context.Periodos.OrderBy(x => x.Orden).Select(x => x.PeriodoDeEjecucion).ToListAsync();
        }

        public async Task<List<Log>> ObtenerLogs()
        {
            var logs = await _context.Logs.OrderByDescending(x => x.Fecha).ToListAsync();

            return logs;
        }

        public async Task GenerarLog(Log log)
        {
            _context.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
