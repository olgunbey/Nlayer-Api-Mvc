using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Core.Services
{
    public interface IProductService:IService<Product>
    {
        Task<CustomResponseDTO<List<ProductCategoryDTO>>> GetProductWithCategoryAsync();
    }
}
