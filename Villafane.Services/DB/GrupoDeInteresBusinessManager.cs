using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;

namespace Villafane.Services.DB
{
    public class GrupoDeInteresBusinessManager
    {
        private readonly VillafaneContext _context;

        public GrupoDeInteresBusinessManager(VillafaneContext context)
        {
            _context = context;
        }

        public async Task<List<GruposDeIntere>> ObtenerTodosAsync()
        {
            var gruposDeInteres = await _context.GruposDeInteres.OrderBy(x=>x.Orden).ToListAsync();

            return gruposDeInteres;
        }

        public async Task Agregar(GruposDeIntere grupo)
        {
            if(grupo.Grupo != null)
            {
                _context.Add(grupo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Editar(GruposDeIntere grupo)
        {
            if(grupo != null && grupo.Id != 0)
            {
                _context.Update(grupo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Eliminar(GruposDeIntere grupo)
        {
            if (grupo != null && grupo.Id != 0)
            {
                _context.Remove(grupo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
