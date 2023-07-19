using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class PesosValoresTeorico
    {
        public int Id { get; set; }
        public int IdGrupoInteres { get; set; }
        public int IdValor { get; set; }
        public decimal ValorTeorico { get; set; }
        public bool? InformacionDisponible { get; set; }
        public decimal? Tb12 { get; set; }
        public decimal? Tb21 { get; set; }
        public decimal? Tb22 { get; set; }
        public decimal? Tb23 { get; set; }
        public decimal? Tb3 { get; set; }
        public decimal? Tb4 { get; set; }
        public string? Periodo { get; set; }

        public virtual GruposDeIntere IdGrupoInteresNavigation { get; set; } = null!;
        public virtual Valore IdValorNavigation { get; set; } = null!;
    }
}
