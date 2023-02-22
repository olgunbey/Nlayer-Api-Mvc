using AutoMapper;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using Nlayer.Core.Repositories;
using Nlayer.Core.Services;
using Nlayer.Core.UnitofWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Services.Services
{
    public class CategoryService : GenericServices<Category>,ICategoryService
    {

        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(IGenericRepository<Category> _genericRepository, IUnitOfWork _unitOfWork, ICategoryRepository _categoryRepository,IMapper _mapper) : base(_genericRepository, _unitOfWork)
        {
            categoryRepository = _categoryRepository;
            mapper = _mapper;
        }

        public async Task<CustomResponseDTO<CategoryWithProductDTO>> GetCategorieswithProductAsync(int categoryid)
        {
            Category category= await  categoryRepository.GetCategorieswithProductAsync(categoryid);
            var categoryDTO=mapper.Map<CategoryWithProductDTO>(category);
            return CustomResponseDTO<CategoryWithProductDTO>.Success(200,categoryDTO);

        }
        public async Task<CustomResponseDTO<List<CategoryWithProductDTO>>> CategoryieswithProductAsync()
        {
            List<Category> categories =await categoryRepository.CategoryiesWithProductAsync();
            List<CategoryWithProductDTO> categoryWithProductDTOs = mapper.Map<List<CategoryWithProductDTO>>(categories);
            return CustomResponseDTO<List<CategoryWithProductDTO>>.Success(200, categoryWithProductDTOs);
        }

        
    }
}
