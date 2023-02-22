using Microsoft.EntityFrameworkCore;
using Nlayer.Core.Entities;
using Nlayer.Core.Repositories;
using Nlayer.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T :class,new()
    {
        protected readonly AppDbContext _appContext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext db)
        {
            _appContext = db;
            _dbSet=_appContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _appContext.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _appContext.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
          return  await _dbSet.AnyAsync(expression);
        }

        public void Delete(T entity)
        {
            _appContext.Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async ValueTask<T> GetByIDAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _appContext.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _appContext.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
