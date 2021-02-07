using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductInOrder> ProductInOrders { get; set; }
        public DbSet<OrderInfo> OrdersInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");
            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<Category>().HasMany<Product>(c => c.Products)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.CategoryRefId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CartItems>().HasKey(cp => new { cp.CartRefId, cp.ProductRefId });
            builder.Entity<CartItems>().HasOne<ShoppingCart>(cp => cp.ShoppingCart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(cp => cp.CartRefId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CartItems>().HasOne<Product>(cp => cp.Products)
                .WithMany(p => p.CartItems)
                .HasForeignKey(cp => cp.ProductRefId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ProductInOrder>().HasKey(cp => new { cp.OrderRefId, cp.ProductRefId });
            builder.Entity<ProductInOrder>().HasOne<Order>(cp => cp.Orders)
                .WithMany(c => c.ProductInOrders)
                .HasForeignKey(cp => cp.OrderRefId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ProductInOrder>().HasOne<Product>(cp => cp.Products)
                .WithMany(p => p.ProductInOrders)
                .HasForeignKey(cp => cp.ProductRefId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>().HasOne(oi => oi.OrderInfos)
                .WithOne(o => o.Orders)
                .HasForeignKey<OrderInfo>(oi => oi.OrderRefId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
