using MarketplaceSync.Domain.Entities;
using MarketplaceSync.Domain.Interfaces;

namespace MarketplaceSync.Service.Product
{
    public interface IProductService
    {
        Task<IEnumerable<Domain.Entities.Product>> GetProductsByUserAsync(Guid userId);
        Task<Domain.Entities.Product> GetProductByIdAsync(Guid productId);
        Task<Domain.Entities.Product> CreateProductAsync(Domain.Entities.Product product);
        Task<bool> UpdateProductAsync(Domain.Entities.Product product);
        Task<bool> DeleteProductAsync(Guid productId);
    }

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Domain.Entities.Product>> GetProductsByUserAsync(Guid userId)
        {
            return await _unitOfWork.Products.FindAsync(p => p.UserId == userId && p.IsActive);
        }

        public async Task<Domain.Entities.Product> GetProductByIdAsync(Guid productId)
        {
            return await _unitOfWork.Products.GetByIdAsync(productId);
        }

        public async Task<Domain.Entities.Product> CreateProductAsync(Domain.Entities.Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return product;
        }

        public async Task<bool> UpdateProductAsync(Domain.Entities.Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            product.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Products.UpdateAsync(product);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                return false;

            product.IsActive = false;
            await _unitOfWork.Products.UpdateAsync(product);
            return await _unitOfWork.CommitAsync();
        }
    }
}