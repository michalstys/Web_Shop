using Microsoft.EntityFrameworkCore;
using Web_Shop.Persistence.Repositories.Interfaces;
using WWSI_Shop.Persistence.MySQL.Context;

namespace Web_Shop.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly WwsishopContext _dbContext;

        internal DbSet<T> dbSet;

        private bool _tracking = true;


        public GenericRepository(WwsishopContext dbContext)
        {
            _dbContext = dbContext;

            dbSet = _dbContext.Set<T>();
        }

        public virtual IQueryable<T> Entities => GetEntities();

        public IGenericRepository<T> WithTracking()
        {
            _tracking = true;
            return this;
        }
        public IGenericRepository<T> WithoutTracking()
        {
            _tracking = false;
            return this;
        }

        public virtual async Task<T?> GetByIdAsync(params object?[]? id)
        {
            if (_tracking)
            {
                return await dbSet.FindAsync(id);
            }
            else
            {
                var entity = await dbSet.FindAsync(id);

                if (entity != null)
                {
                    _dbContext.Entry(entity).State = EntityState.Detached;
                }

                return entity;
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity, params object?[]? id)
        {
            var task = await Task.Run(() => _dbContext.Update(entity));

            return entity;
        }

        public virtual Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);

            return Task.CompletedTask;
        }

        public virtual async Task<bool> Exists(params object?[]? id)
        {
            var entity = await dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }

            return entity != null;
        }

        private IQueryable<T> GetEntities()
        {
            if (_tracking)
                return dbSet;
            else
                return dbSet.AsNoTracking();
        }
    }
}
