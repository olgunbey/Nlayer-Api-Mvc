using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nlayer.Core.DTOs;

namespace Nlayer.API.ValidationFilter
{
    public class ValidateFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context) //client uzerinde validasyon yapar, actiona girmeden once 
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x=>x.ErrorMessage).ToList();

                context.Result = new BadRequestObjectResult(CustomResponseDTO<CustomNoContentResponseDTO>.Fail(errors, 400));
            }
        }
    }
}
