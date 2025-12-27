using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Sku { get; set; } // Identificador único do produto
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relationships
        public User User { get; set; }
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
