using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villafane.Domain.ViewModels
{
    
    public class DescargarExcelIndicadoresViewModel
    {
        public int IDGDI { get; set; }

        public int IDVARIABLE { get; set; }

        public string PERIODO { get; set; }
        public string NOMBREINDICADOR { get; set; }
        public string FUENTE { get; set; }
        public int AÑO { get; set; }
        public int AÑOUSO { get; set; }
        public string ESCALA { get; set; }
        public decimal? MINESCALA { get; set; }
        public decimal? MAXESCALA { get; set; }

        public decimal? DATO { get; set; }
        public decimal? DATONORMALIZADO { get; set; }
    }

    public class DescargarExcelListadoGDI
    {
        public int IDGDI { get; set; }
        public string GDI { get; set; }
    }
    public class DescargarExcelListadoVariables
    {
        public int IDVARIABLE { get; set; }
        public string VARIABLE { get; set; }
        public string VALOR { get; set; }
    }
}
