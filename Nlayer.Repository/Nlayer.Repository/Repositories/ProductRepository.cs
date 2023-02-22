using Microsoft.EntityFrameworkCore;
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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            return await _appContext.Products.Include(x=> x.Category).ToListAsync();
        }
    }
}
