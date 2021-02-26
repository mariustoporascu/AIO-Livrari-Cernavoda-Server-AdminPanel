using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OShop.Domain.Models;

#nullable disable

namespace OShop.Database
{
    public partial class OnlineShopDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductInOrder> ProductInOrders { get; set; }
        public DbSet<OrderInfo> OrdersInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasMany<Product>(c => c.Products)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.CategoryRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CartItems>().HasKey(cp => new { cp.CartRefId, cp.ProductRefId });
            modelBuilder.Entity<CartItems>().HasOne<ShoppingCart>(cp => cp.ShoppingCart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(cp => cp.CartRefId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CartItems>().HasOne<Product>(cp => cp.Products)
                .WithMany(p => p.CartItems)
                .HasForeignKey(cp => cp.ProductRefId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProductInOrder>().HasKey(cp => new { cp.OrderRefId, cp.ProductRefId });
            modelBuilder.Entity<ProductInOrder>().HasOne<Order>(cp => cp.Orders)
                .WithMany(c => c.ProductInOrders)
                .HasForeignKey(cp => cp.OrderRefId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProductInOrder>().HasOne<Product>(cp => cp.Products)
                .WithMany(p => p.ProductInOrders)
                .HasForeignKey(cp => cp.ProductRefId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>().HasOne(oi => oi.OrderInfos)
                .WithOne(o => o.Orders)
                .HasForeignKey<OrderInfo>(oi => oi.OrderRefId)
                .OnDelete(DeleteBehavior.Cascade);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
