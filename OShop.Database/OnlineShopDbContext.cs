using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OShop.Domain.Models;

#nullable disable

namespace OShop.Database
{
    public partial class OnlineShopDbContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options
            )
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Restaurant> Restaurante { get; set; }
        public DbSet<SuperMarket> SuperMarkets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductInOrder> ProductInOrders { get; set; }
        public DbSet<OrderInfo> OrdersInfos { get; set; }
        public DbSet<MeasuringUnit> MeasuringUnits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasMany<Product>(c => c.Products)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.CategoryRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>().HasMany<SubCategory>(c => c.SubCategories)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.CategoryRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SuperMarket>().HasMany<Product>(c => c.Products)
                .WithOne(p => p.SuperMarkets)
                .HasForeignKey(p => p.SuperMarketRefId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Restaurant>().HasMany<Product>(c => c.Products)
                .WithOne(p => p.Restaurante)
                .HasForeignKey(p => p.RestaurantRefId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SuperMarket>().HasMany<Category>(c => c.Categories)
                .WithOne(p => p.SuperMarkets)
                .HasForeignKey(p => p.SuperMarketRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Restaurant>().HasMany<Category>(c => c.Categories)
                .WithOne(p => p.Restaurante)
                .HasForeignKey(p => p.RestaurantRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CartItems>().HasKey(cp => new { cp.CartRefId, cp.ProductRefId });
            modelBuilder.Entity<CartItems>().HasOne<ShoppingCart>(cp => cp.ShoppingCart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(cp => cp.CartRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CartItems>().HasOne<Product>(cp => cp.Products)
                .WithMany(p => p.CartItems)
                .HasForeignKey(cp => cp.ProductRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductInOrder>().HasKey(cp => new { cp.OrderRefId, cp.ProductRefId });
            modelBuilder.Entity<ProductInOrder>().HasOne<Order>(cp => cp.Orders)
                .WithMany(c => c.ProductInOrders)
                .HasForeignKey(cp => cp.OrderRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductInOrder>().HasOne<Product>(cp => cp.Products)
                .WithMany(p => p.ProductInOrders)
                .HasForeignKey(cp => cp.ProductRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>().HasOne(oi => oi.OrderInfos)
                .WithOne(o => o.Orders)
                .HasForeignKey<OrderInfo>(oi => oi.OrderRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>()
            .HasMany<Order>(appUser => appUser.DriverOrders)
            .WithOne(tId => tId.Driver)
            .HasForeignKey(tId => tId.DriverRefId)
            .OnDelete(DeleteBehavior.NoAction);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
