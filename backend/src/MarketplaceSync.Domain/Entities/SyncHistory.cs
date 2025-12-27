using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Entities
{
    public class SyncHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string MarketplaceType { get; set; }
        public string SyncType { get; set; } // Estoque, Pedidos
        public string Status { get; set; } // Sucesso, Erro, Pendente
        public string ErrorMessage { get; set; }
        public int ItemsSynced { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }

        // Relationships
        public User User { get; set; }
    }
}
