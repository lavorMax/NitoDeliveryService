using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NiteDeliveryService.Shared.DAL.Implemetations
{
    public class BaseRepository<T, K> : IBaseRepository<T, K> where T : BaseEntity<K>
    {
        protected readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            this._context = context;
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                var result = await _context.Set<T>().AddAsync(entity).ConfigureAwait(false);

                return result.Entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<T> Read(K id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                T result = await Read(entity.Id).ConfigureAwait(false);
                if(result != null)
                {
                    _context.Entry(result).CurrentValues.SetValues(entity);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Delete(K id)
        {
            try
            {
                T result = await Read(id).ConfigureAwait(false);
                if (result != null)
                {
                    _context.Set<T>().Remove(result);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _context.Set<T>().ToListAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
