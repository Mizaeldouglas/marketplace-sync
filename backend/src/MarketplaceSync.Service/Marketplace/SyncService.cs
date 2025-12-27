using MarketplaceSync.Domain.Entities;
using MarketplaceSync.Domain.Interfaces;

namespace MarketplaceSync.Service.Marketplace
{
    public interface ISyncService
    {
        Task<bool> SyncStockAsync(Guid userId, string marketplace);
        Task<bool> SyncOrdersAsync(Guid userId, string marketplace);
        Task<SyncHistory> GetLastSyncAsync(Guid userId, string marketplace, string syncType);
    }

    public class SyncService : ISyncService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SyncService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SyncStockAsync(Guid userId, string marketplace)
        {
            try
            {
                var syncHistory = new SyncHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    MarketplaceType = marketplace,
                    SyncType = "Estoque",
                    Status = "Pendente",
                    StartedAt = DateTime.UtcNow,
                    ItemsSynced = 0
                };

                await _unitOfWork.SyncHistories.AddAsync(syncHistory);

                // TODO: Implementar lógica real de sincronização
                // 1. Buscar MarketplaceAccount do usuário
                // 2. Chamar API do marketplace
                // 3. Atualizar Stock no banco

                syncHistory.Status = "Sucesso";
                syncHistory.FinishedAt = DateTime.UtcNow;
                await _unitOfWork.SyncHistories.UpdateAsync(syncHistory);
                return await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }

        public async Task<bool> SyncOrdersAsync(Guid userId, string marketplace)
        {
            try
            {
                var syncHistory = new SyncHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    MarketplaceType = marketplace,
                    SyncType = "Pedidos",
                    Status = "Pendente",
                    StartedAt = DateTime.UtcNow,
                    ItemsSynced = 0
                };

                await _unitOfWork.SyncHistories.AddAsync(syncHistory);

                // TODO: Implementar lógica real de sincronização de pedidos

                syncHistory.Status = "Sucesso";
                syncHistory.FinishedAt = DateTime.UtcNow;
                await _unitOfWork.SyncHistories.UpdateAsync(syncHistory);
                return await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }

        public async Task<SyncHistory> GetLastSyncAsync(Guid userId, string marketplace, string syncType)
        {
            var syncs = await _unitOfWork.SyncHistories.FindAsync(s =>
                s.UserId == userId &&
                s.MarketplaceType == marketplace &&
                s.SyncType == syncType);

            return syncs.OrderByDescending(s => s.StartedAt).FirstOrDefault();
        }
    }
}