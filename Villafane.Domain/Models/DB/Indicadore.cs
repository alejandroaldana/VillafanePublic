using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Indicadore
    {
        public int Id { get; set; }
        public string Indicador { get; set; } = null!;
        public int IdGrupoInteres { get; set; }
        public int IdVariable { get; set; }
        public decimal Dato { get; set; }
        public string? Fuente { get; set; }
        public int? FechaIndicador { get; set; }
        public int? FechaUsoIndicador { get; set; }
        public int? IdEscala { get; set; }
        public decimal? DatoNormalizado { get; set; }
        public decimal? PesoIndicadorEnIr { get; set; }
        public decimal? PesoIndicadorEnVariable { get; set; }
        public decimal? PesoIndicadorEnVariableNormalizado { get; set; }
        public decimal? NotaDeLaVariable { get; set; }
        public decimal? PesoIndicadorEnValor { get; set; }
        public decimal? PesoIndicadorEnValorNormalizado { get; set; }
        public decimal? NotaDelValor { get; set; }
        public decimal? PesoIndicadorEnGdi { get; set; }
        public decimal? PesoIndicadorEnGdinormalizado { get; set; }
        public decimal? PesoDelGdienValor { get; set; }
        public string? Periodo { get; set; }

        public virtual Escala? IdEscalaNavigation { get; set; }
        public virtual GruposDeIntere IdGrupoInteresNavigation { get; set; } = null!;
        public virtual Variable IdVariableNavigation { get; set; } = null!;
    }
}
