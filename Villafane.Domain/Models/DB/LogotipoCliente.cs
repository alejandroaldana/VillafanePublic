using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class LogotipoCliente
    {
        public LogotipoCliente()
        {
            Configuracions = new HashSet<Configuracion>();
        }

        public int Id { get; set; }
        public byte[] BinarioLogo { get; set; } = null!;
        public string ContentTypeLogo { get; set; } = null!;
        public string ExtensionLogo { get; set; } = null!;

        public virtual ICollection<Configuracion> Configuracions { get; set; }
    }
}
