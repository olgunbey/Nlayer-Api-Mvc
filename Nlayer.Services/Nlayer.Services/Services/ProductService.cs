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
    public class ProductService : GenericServices<Product>, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;
        public ProductService(IGenericRepository<Product> _genericRepository, IUnitOfWork _unitOfWork,IMapper mapper,IProductRepository productRepository) : base(_genericRepository, _unitOfWork)
        {
            _mapper = mapper;
            _repository = productRepository;
        }

        public async Task<CustomResponseDTO<List<ProductCategoryDTO>>>GetProductWithCategoryAsync()
        {
            List<Product> productswithcategory=await _repository.GetProductsWithCategoryAsync();
            List<ProductCategoryDTO> dto = _mapper.Map<List<ProductCategoryDTO>>(productswithcategory);
            return  CustomResponseDTO<List<ProductCategoryDTO>>.Success(200,dto);
        }
    }
}
