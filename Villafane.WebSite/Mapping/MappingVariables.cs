using Villafane.Domain.Models.DB;
using Villafane.Domain.ViewModels;
using Villafane.WebSite.Models;

namespace Villafane.WebSite.Mapping
{
    public class MappingVariables
    {

        public static VariableViewModel VariablesAViewModel(Variable model)
        {
            VariableViewModel result = new VariableViewModel()
            {
                Id = model.Id,
               Variable1 = model.Variable1,
                IdValor = model.IdValor,
                

            };

            
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

        public static List<VariableViewModel> VariablesAViewModel(List<Variable> model)
        {
            List<VariableViewModel> resultado = new List<VariableViewModel>();

            foreach (var item in model)
            {
                var elemento = VariablesAViewModel(item);

                resultado.Add(elemento);
            }

            return resultado;
        }

    }
}
