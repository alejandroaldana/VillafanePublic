using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Configuracion
    {
        public int Id { get; set; }
        public int? IdLogo { get; set; }
        public string? ColorPrincipal { get; set; }
        public string? ColorBarra { get; set; }
        public string? ColorCabecerasTablas { get; set; }
        public string? ColorCabeceraTablasVertical { get; set; }
        public bool CambiarModoCalculo { get; set; }

        public virtual LogotipoCliente? IdLogoNavigation { get; set; }
    }
}
