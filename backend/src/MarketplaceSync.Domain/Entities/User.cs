namespace MarketplaceSync.Domain.Entities
{
    public class User
    {

        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string LojaName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;


        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<MarketplaceAccount> MarketplaceAccounts { get; set; } = new List<MarketplaceAccount>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<SyncHistory> SyncHistories { get; set; } = new List<SyncHistory>();

    }
}
