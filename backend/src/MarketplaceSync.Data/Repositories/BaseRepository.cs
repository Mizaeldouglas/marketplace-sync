using Microsoft.EntityFrameworkCore;
using MarketplaceSync.Domain.Interfaces;
using MarketplaceSync.Data.Context;

namespace MarketplaceSync.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly MarketplaceSyncContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(MarketplaceSyncContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
        {
            return await Task.FromResult(_dbSet.Where(predicate).ToList());
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}