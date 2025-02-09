using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace AvaluoAPI.Middlewares
{
    public class ValidationFilterDTO : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Filtra los errores obtenidos con las validaciones del DTO para que traiga el mismo formato que el Middleware 
            if (!context.ModelState.IsValid)
            {
                var errorMessages = context.ModelState
                    .Where(m => m.Value.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                var response = new
                {
                    StatusCode = 400,
                    Message = string.Join(" | ", errorMessages) // 🔹 Concatena errores con " | "
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
