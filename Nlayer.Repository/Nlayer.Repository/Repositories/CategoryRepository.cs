using Microsoft.EntityFrameworkCore;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using Nlayer.Core.Repositories;
using Nlayer.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext db) : base(db)
        {
        }

        public  Task<List<Category>> CategoryiesWithProductAsync()
        {
            return Task.FromResult(_appContext.Categories.Include(c => c.Products).ToList());
        }

        public async Task<Category> GetCategorieswithProductAsync(int categoryid)
        {
            return await _appContext.Categories.Include(x => x.Products).SingleOrDefaultAsync(x=>x.EntityID==categoryid);
        }
    }
}
