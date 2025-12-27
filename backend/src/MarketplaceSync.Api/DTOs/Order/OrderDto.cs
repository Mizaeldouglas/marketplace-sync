namespace MarketplaceSync.Api.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string MarketplaceOrderId { get; set; }
        public string MarketplaceName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}