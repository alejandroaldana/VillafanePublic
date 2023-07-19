using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Periodo
    {
        public int Id { get; set; }
        public string PeriodoDeEjecucion { get; set; } = null!;
        public int Orden { get; set; }
    }
}
