namespace MarketplaceSync.Api.DTOs.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal Margin => Cost > 0 ? ((Price - Cost) / Cost * 100) : 0;
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateProductDto
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
