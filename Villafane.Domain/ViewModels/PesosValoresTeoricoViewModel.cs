using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;

namespace Villafane.Domain.ViewModels
{
    public class PesosValoresTeoricoViewModel
    {
        public int Id { get; set; }
        public int IdGrupoInteres { get; set; }
        public int IdValor { get; set; }
        public decimal ValorTeorico { get; set; }
        public bool? InformacionDisponible { get; set; }
        public decimal? Tb21 { get; set; }
        public decimal? Tb22 { get; set; }
        public decimal? Tb23 { get; set; }
        public decimal? Tb3 { get; set; }
        public decimal? Tb4 { get; set; }

        public virtual GrupoDeInteresViewModel IdGrupoInteresNavigation { get; set; } = null!;
        public virtual ValorViewModel IdValorNavigation { get; set; } = null!;
    }
}
