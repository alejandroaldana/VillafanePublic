using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Escala
    {
        public Escala()
        {
            Indicadores = new HashSet<Indicadore>();
        }

        public int Id { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Indicadore> Indicadores { get; set; }
    }
}
