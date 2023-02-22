using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Core.Repositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        public Task<Category> GetCategorieswithProductAsync(int categoryid);
        public Task<List<Category>> CategoryiesWithProductAsync();
    }
}
