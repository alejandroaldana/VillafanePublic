using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villafane.Domain.ViewModels
{
    public class IndicadoresExcelViewModel
    {

        public string GDI { get; set; }
        public string VALOR { get; set; }
        public string VARIABLE { get; set; }
        public string NOMBRE_INDICADOR { get; set; }
        public string FUENTE { get; set; }
        public string AÑO { get; set; }
        public string AÑO_USO { get; set; }
        public string ESCALA { get; set; }
        public string MIN_ESCALA { get; set; }
        public string MAX_ESCALA { get; set; }
        public string DATO { get; set; }
        public string DATO_NORMALIZADO { get; set; }
        public string PERIODO { get; set; }
    }
}
