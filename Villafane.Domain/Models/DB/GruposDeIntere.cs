using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class GruposDeIntere
    {
        public GruposDeIntere()
        {
            Indicadores = new HashSet<Indicadore>();
            PesosValoresDefinitivos = new HashSet<PesosValoresDefinitivo>();
            PesosValoresTeoricos = new HashSet<PesosValoresTeorico>();
            PesosVariablesTeoricos = new HashSet<PesosVariablesTeorico>();
        }

        public int Id { get; set; }
        public string Grupo { get; set; } = null!;
        public int? Orden { get; set; }
        public decimal? PesoEnIr { get; set; }

        public virtual ICollection<Indicadore> Indicadores { get; set; }
        public virtual ICollection<PesosValoresDefinitivo> PesosValoresDefinitivos { get; set; }
        public virtual ICollection<PesosValoresTeorico> PesosValoresTeoricos { get; set; }
        public virtual ICollection<PesosVariablesTeorico> PesosVariablesTeoricos { get; set; }
    }
}
