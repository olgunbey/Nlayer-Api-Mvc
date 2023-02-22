using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nlayer.API.ValidationFilter;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using Nlayer.Core.Services;
using Nlayer.Core.UnitofWorks;
using System.Runtime.CompilerServices;

namespace Nlayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public CategoriesController(ICategoryService _categoryService, IMapper _mapper)
        {
            categoryService= _categoryService;
            mapper= _mapper;
        }
        [ServiceFilter(typeof(CategoryIDFilter))]
        [HttpGet("[action]/{categoryid}")]
        public async Task<IActionResult> CategorieswithProducts(int categoryid)
        {
            return new CustomResponseDTO<CategoryWithProductDTO>.Response<CategoryWithProductDTO>().Fonksiyon(await categoryService.GetCategorieswithProductAsync(categoryid));
           
        }
        [HttpGet]
        public async Task<IActionResult> CategorieswithProducts()
        {
            return new CustomResponseDTO<List<CategoryWithProductDTO>>.Response<List<CategoryWithProductDTO>>().Fonksiyon(await categoryService.CategoryieswithProductAsync());
        }
        [HttpDelete("[action]/{categoryid}")]
        //cascade silinmesi otomatik aktif oldugu için karşısındaki product' verilerinide siler.
        public async Task<IActionResult> CategoriesDelete(int categoryid)
        {
           Category category=await categoryService.GetByIDAsync(categoryid);
           await categoryService.DeleteAsync(category);
           return new CustomResponseDTO<CustomNoContentResponseDTO>.Response<CustomNoContentResponseDTO>().Fonksiyon(CustomResponseDTO<CustomNoContentResponseDTO>.Success(200));
        }
        [HttpPut]
        public async Task<IActionResult> CategoriesUpdate(CategoryDTO _category)
        {
          await categoryService.UpdateAsync(mapper.Map<Category>(_category));
          return new CustomResponseDTO<CustomNoContentResponseDTO>.Response<CustomNoContentResponseDTO>().Fonksiyon(CustomResponseDTO<CustomNoContentResponseDTO>.Success(204));
          
        }
       
        [HttpPost]
        public async Task<IActionResult> CategoriesSave(CategoryDTO _category)
        {
            await categoryService.AddAsync(mapper.Map<Category>(_category));
            return new CustomResponseDTO<CustomNoContentResponseDTO>.Response<CustomNoContentResponseDTO>().Fonksiyon(CustomResponseDTO<CustomNoContentResponseDTO>.Success(202));
        }
        [HttpGet("[action]/{categoryname}")]
        public async Task<IActionResult> CategoriesWhere(string categoryname)
        {

            List<Category> categorylist = categoryService.Where(x => x.Name == categoryname).ToList();
            List<CategoryDTO> categoryDTOs = mapper.Map<List<CategoryDTO>>(categorylist);
            return new CustomResponseDTO<List<CategoryDTO>>.Response<List<CategoryDTO>>().Fonksiyon(CustomResponseDTO<List<CategoryDTO>>.Success(200, categoryDTOs));
        }


    }
}
