using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villafane.Domain.ViewModels
{
    public class DescargarExcelPesosVariablesViewModel
    {
        public int IdGDI { get; set; }
        public string GDI { get; set; }

        public int IdVariable { get; set; }
        public string Variable { get; set; }
        public string Periodo { get; set; }

        public decimal? Dato { get; set; }
    }
}
