using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var valueProvider = bindingContext.ValueProvider.GetValue(propertyName);

            if (valueProvider == ValueProviderResult.None) return Task.CompletedTask;

            try
            {
                var deserializeValue = JsonConvert.DeserializeObject<T>(valueProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializeValue);
            }
            catch
            {

                bindingContext.ModelState.TryAddModelError(propertyName, "Invalid Value for type List<int>");
            }

            return Task.CompletedTask;
        }
    }
}
