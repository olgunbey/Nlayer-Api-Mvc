using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Core.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        IQueryable<T> GetAll();
        ValueTask<T> GetByIDAsync(int id);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
    }
}
