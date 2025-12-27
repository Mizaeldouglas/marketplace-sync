using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Entities
{
    public class MarketplaceAccount
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string MarketplaceType { get; set; } // MercadoLivre, Shopee, Amazon, Magalu
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string SellerIdOnMarketplace { get; set; }
        public DateTime TokenExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relationships
        public User User { get; set; }
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
