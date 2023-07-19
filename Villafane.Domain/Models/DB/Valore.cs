using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Valore
    {
        public Valore()
        {
            PesosValoresTeoricos = new HashSet<PesosValoresTeorico>();
            Variables = new HashSet<Variable>();
        }

        public int Id { get; set; }
        public string Valor { get; set; } = null!;
        public int? Orden { get; set; }

        public virtual ICollection<PesosValoresTeorico> PesosValoresTeoricos { get; set; }
        public virtual ICollection<Variable> Variables { get; set; }
    }
}
