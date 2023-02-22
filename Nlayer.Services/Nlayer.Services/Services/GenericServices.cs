using Microsoft.EntityFrameworkCore;
using Nlayer.Core.Entities;
using Nlayer.Core.Repositories;
using Nlayer.Core.Services;
using Nlayer.Core.UnitofWorks;
using Nlayer.Repository.Context;
using Nlayer.Repository.Repositories;
using Nlayer.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Services.Services;

public class GenericServices<T> : IService<T> where T :  BaseEntity, new()
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IGenericRepository<T> genericRepository;
    public GenericServices(IGenericRepository<T> _genericRepository, IUnitOfWork _unitOfWork)
    {
        unitOfWork= _unitOfWork;
        genericRepository = _genericRepository;
    }

    public async Task AddAsync(T entity)
    {
        await genericRepository.AddAsync(entity);
        await unitOfWork.CommitAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
       await genericRepository.AddRangeAsync(entities);
        await unitOfWork.CommitAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
     return await  genericRepository.AnyAsync(expression);
    }

    public async Task DeleteAsync(T entity)
    {
        genericRepository.Delete(entity);
      await  unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
      return await genericRepository.GetAll().ToListAsync();
    }

    public async ValueTask<T> GetByIDAsync(int id)
    {
        T entity =await genericRepository.GetByIDAsync(id);
        if (entity == null)
        {
            throw new NotFoundException($"{nameof(T)} hata, veri yok");
        }
        return entity;
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        genericRepository.RemoveRange(entities);
        await unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        genericRepository.Update(entity);
        await unitOfWork.CommitAsync();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
      return  genericRepository.Where(expression);
    }
}
