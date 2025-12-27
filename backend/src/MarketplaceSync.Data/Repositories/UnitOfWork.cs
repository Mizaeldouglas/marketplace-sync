using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketplaceSync.Domain.Entities;
using MarketplaceSync.Domain.Interfaces;
using MarketplaceSync.Data.Context;

namespace MarketplaceSync.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketplaceSyncContext _context;
        private IRepository<User> _users;
        private IRepository<Product> _products;
        private IRepository<Stock> _stocks;
        private IRepository<MarketplaceAccount> _marketplaceAccounts;
        private IRepository<Order> _orders;
        private IRepository<SyncHistory> _syncHistories;

        public UnitOfWork(MarketplaceSyncContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<User> Users => _users ??= new BaseRepository<User>(_context);
        public IRepository<Product> Products => _products ??= new BaseRepository<Product>(_context);
        public IRepository<Stock> Stocks => _stocks ??= new BaseRepository<Stock>(_context);
        public IRepository<MarketplaceAccount> MarketplaceAccounts =>
            _marketplaceAccounts ??= new BaseRepository<MarketplaceAccount>(_context);
        public IRepository<Order> Orders => _orders ??= new BaseRepository<Order>(_context);
        public IRepository<SyncHistory> SyncHistories =>
            _syncHistories ??= new BaseRepository<SyncHistory>(_context);

        public async Task<bool> CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}