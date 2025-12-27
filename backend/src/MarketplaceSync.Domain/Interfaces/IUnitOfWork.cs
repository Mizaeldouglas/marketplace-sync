using MarketplaceSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Stock> Stocks { get; }
        IRepository<MarketplaceAccount> MarketplaceAccounts { get; }
        IRepository<Order> Orders { get; }
        IRepository<SyncHistory> SyncHistories { get; }

        Task<bool> CommitAsync();
    }
}
