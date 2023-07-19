using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Villafane.WebSite.Binder
{
    public class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(decimal))
            {
                return new DecimalModelBinder();
            }

            return null;
        }
    }

    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            //Primero obtenemos el separador de miles para procesar la información aqui no nos interesa
            //el separador decimal porque a este es ya core y eso lo maneja perfecto el framework
            var separadormiles = valueProviderResult.Culture.NumberFormat.CurrencyGroupSeparator;

            //Si es nulo pues finaliza la clase sin hacer más
            if (valueProviderResult == null)
            {
                return Task.CompletedTask;
            }

            //obtenmos el valor del modelo 
            string value = valueProviderResult.FirstValue;

            //si no hay valor finaliza si hacer nada
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // Removemos comas o puntos (separador de miles) nada mas
            // los decimales no importa si es separado por coma o punto 
            // dependiendo la region ya que eso lo manipula bien core
            value = value.Replace(separadormiles, string.Empty).Trim();

            decimal myValue = Convert.ToDecimal(value);

            //retornamos el valor que si se manipula perfectamente en el controller
            //mapping o clase
            bindingContext.Result = ModelBindingResult.Success(myValue);
            return Task.CompletedTask;
        }
    }
}
