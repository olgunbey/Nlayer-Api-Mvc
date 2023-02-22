using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using Nlayer.Core.Repositories;
using Nlayer.Core.Services;
using Nlayer.Core.UnitofWorks;
using Nlayer.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Caching
{
    public class CategoryServiceWithCaching : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICategoryRepository categoryRepository;
        private const string cachingkey = "cachingkeys";
        private readonly IMemoryCache memoryCache;

        public CategoryServiceWithCaching(IMapper _mapper,IUnitOfWork _unitOfWork, ICategoryRepository _categoryRepository,IMemoryCache _memoryCache)
        {
            mapper = _mapper;
            unitOfWork = _unitOfWork;
            categoryRepository = _categoryRepository;
            memoryCache = _memoryCache;
            if(!memoryCache.TryGetValue(cachingkey, out _))
            {
                memoryCache.Set(cachingkey,categoryRepository.CategoryiesWithProductAsync().Result); //buraya product ve category'i toplu getirilecek fonksiyon yazılıp set edilecek
            }
        }
        public async Task AddAsync(Category entity)
        {
            await categoryRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();
            await CachingCategoryData();
        }

        public async Task AddRangeAsync(IEnumerable<Category> entities)
        {
            await categoryRepository.AddRangeAsync(entities);
            await unitOfWork.CommitAsync();
            await CachingCategoryData();
        }

        public  Task<bool> AnyAsync(Expression<Func<Category, bool>> expression)
        {
            return Task.FromResult(memoryCache.Get<List<Category>>(cachingkey).Where(expression.Compile()).Any());
        }

        public async Task DeleteAsync(Category entity)
        {
            categoryRepository.Delete(entity);
            unitOfWork.Commit();
            await CachingCategoryData();
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            IEnumerable<Category> category = memoryCache.Get<IEnumerable<Category>>(cachingkey);
            if (category == null)
            {
                throw new NotFoundException("Category is required");
            }
            return Task.FromResult(category);

        }

        public async ValueTask<Category> GetByIDAsync(int id)
        {
            Category category=memoryCache.Get<List<Category>>(cachingkey).FirstOrDefault(x=>x.EntityID==id);
            if (category == null)
            {
                throw new NotFoundException("Category is required");
            }
            return category;
        }

        public  Task<CustomResponseDTO<CategoryWithProductDTO>> GetCategorieswithProductAsync(int categoryid)
        {
            Category categories = memoryCache.Get<List<Category>>(cachingkey).FirstOrDefault(x=>x.EntityID==categoryid);
            CategoryWithProductDTO categoryWithProductDTO = mapper.Map<CategoryWithProductDTO>(categories);
            return Task.FromResult(CustomResponseDTO<CategoryWithProductDTO>.Success(200,categoryWithProductDTO));
        }

        public async Task RemoveRangeAsync(IEnumerable<Category> entities)
        {
            categoryRepository.RemoveRange(entities);
            unitOfWork.Commit();
         await CachingCategoryData();
        }

        public async Task UpdateAsync(Category entity)
        {
            categoryRepository.Update(entity);
            unitOfWork.Commit();
           await CachingCategoryData();
        }

        public IQueryable<Category> Where(Expression<Func<Category, bool>> expression)
        {
          return  categoryRepository.Where(expression);
        }
        public async Task CachingCategoryData()
        {
            memoryCache.Set(cachingkey,await categoryRepository.GetAll().ToListAsync());
        }

        

        public Task<CustomResponseDTO<List<CategoryWithProductDTO>>> CategoryieswithProductAsync()
        {
            List<Category> categories = memoryCache.Get<List<Category>>(cachingkey);
            List<CategoryWithProductDTO> categoryWithProductDTOs = mapper.Map<List<CategoryWithProductDTO>>(categories);
            return Task.FromResult(CustomResponseDTO<List<CategoryWithProductDTO>>.Success(200, categoryWithProductDTOs));
        }
    }
}
