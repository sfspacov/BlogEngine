using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Shared.Helpers;
using BlogEngine.Core.Common.Exceptions;
using BlogEngine.Core.Data.Entities.Common;

namespace BlogEngine.Core.Services.Implementations
{
    public class Repository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            Preconditions.NotNull(entity, typeof(TEntity).Name);

            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();

            return entity.ID;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            Preconditions.NotNull(entity, typeof(TEntity).Name);

            _dbSet.Update(entity);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            TEntity entityFromDb = await GetByIdAsync(id);

            NullCheckThrowNotFoundException(entityFromDb);

            _dbSet.Remove(entityFromDb);
            await SaveChangesAsync();
        }

        protected void NullCheckThrowNotFoundException(TEntity entity)
        {
            if (entity is null)
            {
                throw new EntityNotFoundException(typeof(TEntity).Name);
            }
        }

        protected async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}