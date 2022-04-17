using Padrao.Domain.Virtual;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Padrao.APi.Filters
{
    public class ValidateModelStateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            List<string> listaErros = new List<string>();
            if (!context.ModelState.IsValid)
            {
                var erros = context.ModelState.Values.SelectMany(e => e.Errors);
                foreach (var erro in erros)
                {
                    var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                    listaErros.Add(errorMsg);
                }
                context.Result = new BadRequestObjectResult(new ResultJson("Objeto Inválido", listaErros.ToList()));
            }
        }
    }
}
