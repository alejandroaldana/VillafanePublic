using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class PesosValoresDefinitivo
    {
        public int Id { get; set; }
        public int IdGrupoInteres { get; set; }
        public int IdVariable { get; set; }
        public decimal ValorDefinitivo { get; set; }

        public virtual GruposDeIntere IdGrupoInteresNavigation { get; set; } = null!;
        public virtual Variable IdVariableNavigation { get; set; } = null!;
    }
}
