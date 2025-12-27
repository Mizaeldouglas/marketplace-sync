using Microsoft.EntityFrameworkCore;
using MarketplaceSync.Domain.Entities;

namespace MarketplaceSync.Data.Context
{
    public class MarketplaceSyncContext : DbContext
    {
        public MarketplaceSyncContext(DbContextOptions<MarketplaceSyncContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MarketplaceAccount> MarketplaceAccounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SyncHistory> SyncHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.LojaName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Sku).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Sku).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Cost).HasPrecision(18, 2);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Products)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Stock Configuration
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MarketplaceProductId).HasMaxLength(255);
                entity.Property(e => e.SyncStatus).HasMaxLength(50);

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.MarketplaceAccount)
                    .WithMany(ma => ma.Stocks)
                    .HasForeignKey(e => e.MarketplaceAccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Índice para buscas rápidas
                entity.HasIndex(e => new { e.ProductId, e.MarketplaceAccountId }).IsUnique();
            });

            // MarketplaceAccount Configuration
            modelBuilder.Entity<MarketplaceAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MarketplaceType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AccessToken).IsRequired();
                entity.Property(e => e.RefreshToken);
                entity.Property(e => e.SellerIdOnMarketplace).IsRequired().HasMaxLength(255);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.MarketplaceAccounts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MarketplaceOrderId).IsRequired().HasMaxLength(255);
                entity.Property(e => e.MarketplaceName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.BuyerName).HasMaxLength(255);
                entity.Property(e => e.BuyerEmail).HasMaxLength(255);
                entity.Property(e => e.ShippingAddress).HasMaxLength(500);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.MarketplaceAccount)
                    .WithMany(ma => ma.Orders)
                    .HasForeignKey(e => e.MarketplaceAccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.MarketplaceOrderId);
            });

            // SyncHistory Configuration
            modelBuilder.Entity<SyncHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MarketplaceType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.SyncType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.SyncHistories)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.StartedAt }).IsDescending(false, true);
            });
        }
    }
}