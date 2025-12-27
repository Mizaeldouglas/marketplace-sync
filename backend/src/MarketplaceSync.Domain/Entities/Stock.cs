using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid MarketplaceAccountId { get; set; }
        public string MarketplaceProductId { get; set; } // ID no marketplace externo
        public int Quantity { get; set; }
        public DateTime LastSyncAt { get; set; } = DateTime.UtcNow;
        public string SyncStatus { get; set; } = "Pendente"; // Pendente, Sincronizado, Erro

        // Relationships
        public Product Product { get; set; }
        public MarketplaceAccount MarketplaceAccount { get; set; }
    }
}
