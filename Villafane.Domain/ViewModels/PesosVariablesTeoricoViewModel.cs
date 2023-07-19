using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villafane.Domain.ViewModels
{
    public class PesosVariablesTeoricoViewModel
    {
        public int IdGrupoInteres { get; set; }
        public int IdVariable { get; set; }
        public decimal ValorTeorico { get; set; }
        public bool InformacionDisponible { get; set; }
        public decimal? Tb21 { get; set; }
        public decimal? Tb22 { get; set; }
        public decimal? Tb23 { get; set; }
        public decimal? Tb3 { get; set; }
        public decimal? Tb4 { get; set; }
        public decimal? Tb12 { get; set; }
        public string? Periodo { get; set; }
    }
}
