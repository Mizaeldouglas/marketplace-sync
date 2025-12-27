namespace MarketplaceSync.Api.DTOs.Marketplace
{
    public class MarketplaceAccountDto
    {
        public Guid Id { get; set; }
        public string MarketplaceType { get; set; }
        public string SellerIdOnMarketplace { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ConnectMarketplaceDto
    {
        public string MarketplaceType { get; set; }
        public string AccessToken { get; set; }
        public string SellerIdOnMarketplace { get; set; }
    }
}