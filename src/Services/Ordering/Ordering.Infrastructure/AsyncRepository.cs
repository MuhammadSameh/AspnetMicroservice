using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderContext _orderContext;

        public AsyncRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _orderContext.Set<T>().AddAsync(entity);
            await _orderContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _orderContext.Set<T>().Remove(entity);
            await _orderContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _orderContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
           return await _orderContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _orderContext.Set<T>();
            if (predicate != null) query = query.Where(predicate);
            
            if (!String.IsNullOrEmpty(includeString)) query = query.Include(includeString);
            if (disableTracking) query = query.AsNoTracking();
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();

        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _orderContext.Set<T>();
            if (predicate != null) query = query.Where(predicate);
            if(includes != null || includes.Count > 0)
            {
               query =  includes.Aggregate(query, (curr, next) => query.Include(next));
            }
            if (disableTracking) query = query.AsNoTracking();
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _orderContext.Set<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _orderContext.Entry(entity).State = EntityState.Modified;
            await _orderContext.SaveChangesAsync();
        }
    }
}
