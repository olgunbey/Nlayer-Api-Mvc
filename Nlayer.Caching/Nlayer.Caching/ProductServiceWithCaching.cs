using AutoMapper;
using Microsoft.AspNetCore.Server.IIS.Core;
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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CachePRoductKey = "productsCache";
        private IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;





        public ProductServiceWithCaching(IUnitOfWork unitOfWork, IProductRepository productRepository, IMemoryCache memoryCache, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.memoryCache = memoryCache;
            this.mapper = mapper;
            if(!memoryCache.TryGetValue(CachePRoductKey,out _)) //burada cacheproduckey adında bir value varmı kontrol eediyor _ olmasının sebebı var mı yok mu kontrolu, normalde sag tarafta istenilen  keyin valuesi döner. dönmemesi için _ yazdık
            {
                memoryCache.Set(CachePRoductKey,productRepository.GetProductsWithCategoryAsync().Result); //productcache key'ine sag parametreleri al 
            }
        }

        public async Task AddAsync(Product entity)
        {
            await productRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Product> entities)
        {
            await productRepository.AddRangeAsync(entities);
            await unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public  Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            List<Product> products = memoryCache.Get<List<Product>>(CachePRoductKey);
            
            return Task.FromResult(products.Where(expression.Compile()).Any());
        }

        public async Task DeleteAsync(Product entity)
        {
            productRepository.Delete(entity);
            await unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
          IEnumerable<Product> procuct= memoryCache.Get<IEnumerable<Product>>(CachePRoductKey);
            if (procuct == null)
            {
                throw new NotFoundException($"{typeof(Product).Name} is required!");
            }
            return Task.FromResult(procuct);
        }

        

        public async Task<CustomResponseDTO<List<ProductCategoryDTO>>> GetProductWithCategoryAsync()
        {
            var cachedata = memoryCache.Get < List < Product >> (CachePRoductKey);
            List<Product> product = await productRepository.GetProductsWithCategoryAsync();
            List<ProductCategoryDTO> dto=mapper.Map<List<ProductCategoryDTO>>(product);
            return CustomResponseDTO<List<ProductCategoryDTO>>.Success(200, dto);
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            productRepository.RemoveRange(entities);
            await unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            productRepository.Update(entity);
            await unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return memoryCache.Get<List<Product>>(CachePRoductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            memoryCache.Set(CachePRoductKey,await productRepository.GetAll().ToListAsync());
        }

        public async ValueTask<Product> GetByIDAsync(int id)
        {
            Product product =   memoryCache.Get<List<Product>>(CachePRoductKey).FirstOrDefault(x => x.EntityID == id);
            if (product == null)
            {
                throw new NotFoundException("Product hata!");
            }
            return product;
            //throw new NotImplementedException();
        }
    }
}
