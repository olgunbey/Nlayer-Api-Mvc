using Microsoft.AspNetCore.Mvc.Filters;
using Nlayer.Core.Entities;
using Nlayer.Core.Services;
using Nlayer.Services.Exceptions;

namespace Nlayer.API.ValidationFilter
{
    public class CategoryIDFilter : IAsyncActionFilter
    {
        private readonly ICategoryService categoryService;
        public CategoryIDFilter(ICategoryService _categoryService)
        {
            categoryService= _categoryService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            object x = context.ActionArguments.Values.FirstOrDefault();
            if (x == null)
            {
                next();
            }
            var y = await categoryService.AnyAsync(y => y.EntityID == (int)x);

            if (!y)
            {
                throw new NotFoundException($"{x} id'ye sahip kategori yok");
            }
            
        }
    }
}
