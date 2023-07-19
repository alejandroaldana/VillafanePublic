using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Variable
    {
        public Variable()
        {
            Indicadores = new HashSet<Indicadore>();
            PesosValoresDefinitivos = new HashSet<PesosValoresDefinitivo>();
            PesosVariablesTeoricos = new HashSet<PesosVariablesTeorico>();
        }

        public int Id { get; set; }
        public string Variable1 { get; set; } = null!;
        public int IdValor { get; set; }

        public virtual Valore IdValorNavigation { get; set; } = null!;
        public virtual ICollection<Indicadore> Indicadores { get; set; }
        public virtual ICollection<PesosValoresDefinitivo> PesosValoresDefinitivos { get; set; }
        public virtual ICollection<PesosVariablesTeorico> PesosVariablesTeoricos { get; set; }
    }
}
