﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using LivroManage.Database;

#nullable disable

namespace LivroManage.Database.Migrations
{
    [DbContext(typeof(OnlineShopDbContext))]
    [Migration("20220621122447_added company hideinapp")]
    partial class addedcompanyhideinapp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("LivroManage.Domain.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("CompanieRefId")
                        .HasColumnType("int");

                    b.Property<bool>("CompleteProfile")
                        .HasColumnType("bit");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasSetPassword")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDriver")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOwner")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LoginToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LoginTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResetTokenPass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResetTokenPassIdentity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserIdentification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("UsernameChangeLimit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("LivroManage.Domain.Models.AvailableCity", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CityId");

                    b.ToTable("AvailableCities");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<int>("CompanieRefId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.HasIndex("CompanieRefId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Companie", b =>
                {
                    b.Property<int>("CompanieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanieId"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Opening")
                        .HasColumnType("datetime2");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefonNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TemporaryClosed")
                        .HasColumnType("bit");

                    b.Property<int>("TipCompanieRefId")
                        .HasColumnType("int");

                    b.Property<bool>("VisibleInApp")
                        .HasColumnType("bit");

                    b.HasKey("CompanieId");

                    b.HasIndex("TipCompanieRefId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.ExtraProdus", b =>
                {
                    b.Property<int>("ExtraProdusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExtraProdusId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductRefId")
                        .HasColumnType("int");

                    b.HasKey("ExtraProdusId");

                    b.HasIndex("ProductRefId");

                    b.ToTable("ExtraProduse");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.FireBaseTokens", b =>
                {
                    b.Property<int>("FBId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FBId"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FBToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FBId");

                    b.HasIndex("UserId");

                    b.ToTable("FBTokens");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.MeasuringUnit", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UnitId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UnitId");

                    b.ToTable("MeasuringUnits");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"), 1L, 1);

                    b.Property<bool>("ClientGaveRatingCompanie")
                        .HasColumnType("bit");

                    b.Property<bool>("ClientGaveRatingDriver")
                        .HasColumnType("bit");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CompanieGaveRating")
                        .HasColumnType("bit");

                    b.Property<int>("CompanieRefId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DriverGaveRating")
                        .HasColumnType("bit");

                    b.Property<string>("DriverRefId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EstimatedTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FinishDelivery")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("HasUserConfirmedET")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOrderPayed")
                        .HasColumnType("bit");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDelivery")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TelephoneOrdered")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalOrdered")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TransportFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserLocationId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("DriverRefId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.OrderInfo", b =>
                {
                    b.Property<int>("OrderInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderInfoId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderRefId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderInfoId");

                    b.HasIndex("OrderRefId")
                        .IsUnique();

                    b.ToTable("OrdersInfos");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gramaj")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int>("MeasuringUnitId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("SubCategoryRefId")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("SubCategoryRefId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.ProductInOrder", b =>
                {
                    b.Property<int>("OrderRefId")
                        .HasColumnType("int");

                    b.Property<int>("ProductRefId")
                        .HasColumnType("int");

                    b.Property<string>("ClientComments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsedQuantity")
                        .HasColumnType("int");

                    b.HasKey("OrderRefId", "ProductRefId");

                    b.HasIndex("ProductRefId");

                    b.ToTable("ProductInOrders");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.RatingClient", b =>
                {
                    b.Property<int>("OrderRefId")
                        .HasColumnType("int");

                    b.Property<string>("UserRefId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RatingDeLaCompanie")
                        .HasColumnType("int");

                    b.Property<int>("RatingDeLaSofer")
                        .HasColumnType("int");

                    b.HasKey("OrderRefId", "UserRefId");

                    b.HasIndex("UserRefId");

                    b.ToTable("RatingClients");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.RatingCompanie", b =>
                {
                    b.Property<int>("OrderRefId")
                        .HasColumnType("int");

                    b.Property<int>("CompanieRefId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("OrderRefId", "CompanieRefId");

                    b.HasIndex("CompanieRefId");

                    b.ToTable("RatingCompanies");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.RatingDriver", b =>
                {
                    b.Property<int>("OrderRefId")
                        .HasColumnType("int");

                    b.Property<string>("DriverRefId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("OrderRefId", "DriverRefId");

                    b.HasIndex("DriverRefId");

                    b.ToTable("RatingDrivers");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.SubCategory", b =>
                {
                    b.Property<int>("SubCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubCategoryId"), 1L, 1);

                    b.Property<int>("CategoryRefId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubCategoryId");

                    b.HasIndex("CategoryRefId");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.TipCompanie", b =>
                {
                    b.Property<int>("TipCompanieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TipCompanieId"), 1L, 1);

                    b.Property<int>("EndHour")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StartHour")
                        .HasColumnType("int");

                    b.HasKey("TipCompanieId");

                    b.ToTable("TipCompanies");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.TransportFee", b =>
                {
                    b.Property<int>("CityRefId")
                        .HasColumnType("int");

                    b.Property<int>("CompanieRefId")
                        .HasColumnType("int");

                    b.Property<int?>("CompanieId")
                        .HasColumnType("int");

                    b.Property<decimal>("MinimumOrderValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TipCompanieRefId")
                        .HasColumnType("int");

                    b.Property<decimal>("TransporFee")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CityRefId", "CompanieRefId");

                    b.HasIndex("CompanieId");

                    b.HasIndex("CompanieRefId");

                    b.ToTable("TransportFees");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.UserLocations", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"), 1L, 1);

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BuildingInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CoordX")
                        .HasColumnType("float");

                    b.Property<double>("CoordY")
                        .HasColumnType("float");

                    b.Property<string>("LocationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("UserLocations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Category", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.Companie", "Companies")
                        .WithMany("Categories")
                        .HasForeignKey("CompanieRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Companies");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Companie", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.TipCompanie", "TipCompanie")
                        .WithMany("Companies")
                        .HasForeignKey("TipCompanieRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TipCompanie");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.ExtraProdus", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.Product", "Products")
                        .WithMany("ExtraProduse")
                        .HasForeignKey("ProductRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Products");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.FireBaseTokens", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", "AppUser")
                        .WithMany("FBTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Order", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", "Driver")
                        .WithMany("DriverOrders")
                        .HasForeignKey("DriverRefId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.OrderInfo", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.Order", "Orders")
                        .WithOne("OrderInfos")
                        .HasForeignKey("LivroManage.Domain.Models.OrderInfo", "OrderRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Product", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.SubCategory", "SubCategory")
                        .WithMany("Products")
                        .HasForeignKey("SubCategoryRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.ProductInOrder", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.Order", "Orders")
                        .WithMany("ProductInOrders")
                        .HasForeignKey("OrderRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivroManage.Domain.Models.Product", "Products")
                        .WithMany("ProductInOrders")
                        .HasForeignKey("ProductRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Orders");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.RatingClient", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.Order", "Orderz")
                        .WithMany("RatingClients")
                        .HasForeignKey("OrderRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", "Users")
                        .WithMany("RatingClients")
                        .HasForeignKey("UserRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Orderz");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.RatingCompanie", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.Companie", "Companies")
                        .WithMany("RatingCompanies")
                        .HasForeignKey("CompanieRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivroManage.Domain.Models.Order", "Orders")
                        .WithMany("RatingCompanies")
                        .HasForeignKey("OrderRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Companies");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.RatingDriver", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", "Driver")
                        .WithMany("RatingDrivers")
                        .HasForeignKey("DriverRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivroManage.Domain.Models.Order", "Orderz")
                        .WithMany("RatingDrivers")
                        .HasForeignKey("OrderRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Orderz");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.SubCategory", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.Category", "Categories")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.TransportFee", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.AvailableCity", "AvailableCities")
                        .WithMany("TransportFees")
                        .HasForeignKey("CityRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivroManage.Domain.Models.Companie", null)
                        .WithMany("TransportFees")
                        .HasForeignKey("CompanieId");

                    b.HasOne("LivroManage.Domain.Models.AvailableCity", "Companii")
                        .WithMany("TransportFees2")
                        .HasForeignKey("CompanieRefId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AvailableCities");

                    b.Navigation("Companii");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.UserLocations", b =>
                {
                    b.HasOne("LivroManage.Domain.Models.ApplicationUser", null)
                        .WithMany("Locations")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.ApplicationUser", b =>
                {
                    b.Navigation("DriverOrders");

                    b.Navigation("FBTokens");

                    b.Navigation("Locations");

                    b.Navigation("RatingClients");

                    b.Navigation("RatingDrivers");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.AvailableCity", b =>
                {
                    b.Navigation("TransportFees");

                    b.Navigation("TransportFees2");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Companie", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("RatingCompanies");

                    b.Navigation("TransportFees");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Order", b =>
                {
                    b.Navigation("OrderInfos");

                    b.Navigation("ProductInOrders");

                    b.Navigation("RatingClients");

                    b.Navigation("RatingCompanies");

                    b.Navigation("RatingDrivers");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.Product", b =>
                {
                    b.Navigation("ExtraProduse");

                    b.Navigation("ProductInOrders");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.SubCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("LivroManage.Domain.Models.TipCompanie", b =>
                {
                    b.Navigation("Companies");
                });
#pragma warning restore 612, 618
        }
    }
}