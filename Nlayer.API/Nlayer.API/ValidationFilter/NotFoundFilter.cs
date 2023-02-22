using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using Nlayer.Core.Services;

namespace Nlayer.API.ValidationFilter
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity  //parametreli actionlar'da işe yarar IAsyncActionFilter
    { //bu class'ın ValidateFilterAttribute ile olan farkı, burada client tarafında hata var ise action'a girmez, önce bu class'ı kontrol eder. 
        private readonly IService<T> service;
        public NotFoundFilter(IService<T> _service)
        {
            service = _service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var data = context.ActionArguments.Values.FirstOrDefault(); //parametredeki degerin ilkini alır

            if(data == null)
            {
                await next();
                return;
            }
            bool byid = await service.AnyAsync(x => x.EntityID == (int)data);

            if(byid)
            {
                await next.Invoke();
                return;
            }
            else
            {
      
                context.Result = new NotFoundObjectResult(CustomResponseDTO<CustomNoContentResponseDTO>.Fail(400, $"{typeof(T)}({(int)(data)}) not found!"));
            }
            

        }
    }
}
