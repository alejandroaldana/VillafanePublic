using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Indicadore
    {
        public decimal DatoNormalizadoCalculado
        {
            get { return ObtenerDatoNormalizado(); }
            
        }

        //public decimal PesoIndicadorEnVariable { get; set; }

        public decimal ObtenerDatoNormalizado()
        {
            var dato = this.Dato;
            if(this.IdEscalaNavigation != null)
            {
                var min = this.IdEscalaNavigation.Min;
                var max = this.IdEscalaNavigation.Max;

                return (0+(dato-min)*((1000-0)/(max-min)));
            }
            else
            {
                return 0;
            }
        }

    }
}
