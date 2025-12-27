using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MarketplaceAccountId { get; set; }
        public string MarketplaceOrderId { get; set; } // ID externo no marketplace
        public string MarketplaceName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pendente, Processado, Enviado, Entregue, Cancelado
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public User User { get; set; }
        public MarketplaceAccount MarketplaceAccount { get; set; }
    }
}
