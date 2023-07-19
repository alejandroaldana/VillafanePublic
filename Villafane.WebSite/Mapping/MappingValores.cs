using Villafane.Domain.Models.DB;
using Villafane.Domain.ViewModels;
using Villafane.WebSite.Models;

namespace Villafane.WebSite.Mapping
{
    public class MappingValores
    {

        public static PesosValoresTeoricoViewModel PesosValoresTeoricoAViewModel(PesosValoresTeorico model)
        {
            PesosValoresTeoricoViewModel result = new PesosValoresTeoricoViewModel()
            {
                Id = model.Id,
                IdGrupoInteres = model.IdGrupoInteres,
                IdValor = model.IdValor,
                ValorTeorico = model.ValorTeorico,
                InformacionDisponible = model.InformacionDisponible,
                Tb21 = model.Tb21,
                Tb22 = model.Tb22,
                Tb23 = model.Tb23,
                Tb3 = model.Tb3,
                Tb4 = model.Tb4,

            };

            if(model.IdGrupoInteresNavigation != null)
            {
                result.IdGrupoInteresNavigation = new GrupoDeInteresViewModel()
                {
                    Id = model.IdGrupoInteresNavigation.Id,
                    Grupo = model.IdGrupoInteresNavigation.Grupo
                };
            }
            if(model.IdValorNavigation != null)
            {
                result.IdValorNavigation = new ValorViewModel()
                {
                    Id = model.IdValorNavigation.Id,
                    Valor = model.IdValorNavigation.Valor
                };
            }

            return result;
        }

        public static List<PesosValoresTeoricoViewModel> PesosValoresTeoricoAViewModel(List<PesosValoresTeorico> model)
        {
            List<PesosValoresTeoricoViewModel> resultado = new List<PesosValoresTeoricoViewModel>();

            foreach (var item in model)
            {
                var elemento = PesosValoresTeoricoAViewModel(item);

                resultado.Add(elemento);
            }

            return resultado;
        }

    }
}
