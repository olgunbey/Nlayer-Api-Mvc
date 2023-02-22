using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Core.Services
{
    public interface ICategoryService:IService<Category>
    {
        Task<CustomResponseDTO<CategoryWithProductDTO>> GetCategorieswithProductAsync(int categoryid);
        Task<CustomResponseDTO<List<CategoryWithProductDTO>>> CategoryieswithProductAsync();
    }
}
