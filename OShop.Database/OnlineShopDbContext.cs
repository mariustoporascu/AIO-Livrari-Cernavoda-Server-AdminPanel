using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductInOrder> ProductInOrders { get; set; }
        public DbSet<OrderInfo> OrdersInfos { get; set; }
        public DbSet<MeasuringUnit> MeasuringUnits { get; set; }
        public DbSet<RatingClient> RatingClients { get; set; }
        public DbSet<RatingDriver> RatingDrivers { get; set; }
        public DbSet<RatingCompanie> RatingCompanies { get; set; }
        public DbSet<UserLocations> UserLocations { get; set; }
        public DbSet<FireBaseTokens> FBTokens { get; set; }
        public DbSet<Companie> Companies { get; set; }
        public DbSet<TipCompanie> TipCompanies { get; set; }
        public DbSet<AvailableCity> AvailableCities { get; set; }
        public DbSet<ExtraProdus> ExtraProduse { get; set; }
        public DbSet<TransportFee> TransportFees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubCategory>().HasMany<Product>(c => c.Products)
                .WithOne(p => p.SubCategory)
                .HasForeignKey(p => p.SubCategoryRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>().HasMany<SubCategory>(c => c.SubCategories)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.CategoryRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>().HasMany<ExtraProdus>(c => c.ExtraProduse)
                .WithOne(p => p.Products)
                .HasForeignKey(p => p.ProductRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Companie>().HasMany<Category>(c => c.Categories)
                .WithOne(p => p.Companies)
                .HasForeignKey(p => p.CompanieRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TipCompanie>().HasMany<Companie>(c => c.Companies)
                .WithOne(p => p.TipCompanie)
                .HasForeignKey(p => p.TipCompanieRefId)
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
            modelBuilder.Entity<AvailableCity>().HasMany<TransportFee>(c => c.TransportFees)
                .WithOne(p => p.AvailableCities)
                .HasForeignKey(p => p.CityRefId)
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
            modelBuilder.Entity<ApplicationUser>()
            .HasMany<FireBaseTokens>(appUser => appUser.FBTokens)
            .WithOne(tId => tId.AppUser)
            .HasForeignKey(tId => tId.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            //rating driver
            modelBuilder.Entity<RatingDriver>().HasKey(cp => new { cp.OrderRefId, cp.DriverRefId });
            modelBuilder.Entity<RatingDriver>().HasOne<Order>(cp => cp.Orderz)
                .WithMany(c => c.RatingDrivers)
                .HasForeignKey(cp => cp.OrderRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RatingDriver>().HasOne<ApplicationUser>(cp => cp.Driver)
                .WithMany(p => p.RatingDrivers)
                .HasForeignKey(cp => cp.DriverRefId)
                .OnDelete(DeleteBehavior.Cascade);
            //rating restaurant
            modelBuilder.Entity<RatingCompanie>().HasKey(cp => new { cp.OrderRefId, cp.CompanieRefId });
            modelBuilder.Entity<RatingCompanie>().HasOne<Order>(cp => cp.Orders)
                .WithMany(c => c.RatingCompanies)
                .HasForeignKey(cp => cp.OrderRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RatingCompanie>().HasOne<Companie>(cp => cp.Companies)
                .WithMany(p => p.RatingCompanies)
                .HasForeignKey(cp => cp.CompanieRefId)
                .OnDelete(DeleteBehavior.Cascade);
            //rating client
            modelBuilder.Entity<RatingClient>().HasKey(cp => new { cp.OrderRefId, cp.UserRefId });
            modelBuilder.Entity<RatingClient>().HasOne<Order>(cp => cp.Orderz)
                .WithMany(c => c.RatingClients)
                .HasForeignKey(cp => cp.OrderRefId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RatingClient>().HasOne<ApplicationUser>(cp => cp.Users)
                .WithMany(p => p.RatingClients)
                .HasForeignKey(cp => cp.UserRefId)
                .OnDelete(DeleteBehavior.Cascade);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
