using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Interfaces
{
    public interface IMarketplaceService
    {
        Task<bool> ValidateTokenAsync(string token);
        Task<List<dynamic>> GetProductsAsync(string sellerId, string token);
        Task<List<dynamic>> GetOrdersAsync(string sellerId, string token);
        Task<bool> UpdateProductStockAsync(string productId, int quantity, string token);
    }
}
