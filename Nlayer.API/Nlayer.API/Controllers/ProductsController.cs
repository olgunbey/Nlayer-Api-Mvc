using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;
using Nlayer.API.ValidationFilter;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using Nlayer.Core.Services;

namespace Nlayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController :ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _service;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _service = productService;
        }

        [HttpGet("GetProductsCategory")]
        public async Task<IActionResult> GetProductsCategory()
        {
            return new CustomResponseDTO<List<ProductCategoryDTO>>.Response<List<ProductCategoryDTO>>().Fonksiyon(await _service.GetProductWithCategoryAsync());
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var product =await _service.GetAllAsync();

            var productDtos = _mapper.Map<List<ProductDTO>>(product.ToList());
            CustomResponseDTO<List<ProductDTO>>.Response<List<ProductDTO>> resp = new();
            return resp.Fonksiyon(CustomResponseDTO<List<ProductDTO>>.Success(200,productDtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        // /api/products/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            Product product = await _service.GetByIDAsync(id);

            ProductDTO productDto = _mapper.Map<ProductDTO>(product);
            return new CustomResponseDTO<ProductDTO>.Response<ProductDTO>().Fonksiyon(CustomResponseDTO<ProductDTO>.Success(200, productDto));
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO)
        {
            await _service.AddAsync(_mapper.Map<Product>(productDTO));
            return new CustomResponseDTO<CustomNoContentResponseDTO>.Response<CustomNoContentResponseDTO>().Fonksiyon(CustomResponseDTO<CustomNoContentResponseDTO>.Success(202));
        }
        [ServiceFilter(typeof(PutFilter))]
        [HttpPut]
        public async Task<IActionResult> Update(ProductDTO productDTO)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDTO));
            return new CustomResponseDTO<CustomNoContentResponseDTO>.Response<CustomNoContentResponseDTO>().Fonksiyon(CustomResponseDTO<CustomNoContentResponseDTO>.Success(203));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            Product product =await _service.GetByIDAsync(id);
            await  _service.DeleteAsync(product);
            return new CustomResponseDTO<CustomNoContentResponseDTO>.Response<CustomNoContentResponseDTO>().Fonksiyon(CustomResponseDTO<CustomNoContentResponseDTO>.Success(206));
        }
     
       
    }
}
