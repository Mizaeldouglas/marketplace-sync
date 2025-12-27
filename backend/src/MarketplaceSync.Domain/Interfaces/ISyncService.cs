using MarketplaceSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Interfaces
{
    public interface ISyncService
    {
        Task SyncStockAsync(Guid userId, string marketplace);
        Task SyncOrdersAsync(Guid userId, string marketplace);
        Task<SyncHistory> GetSyncHistoryAsync(Guid syncHistoryId);
    }
}
