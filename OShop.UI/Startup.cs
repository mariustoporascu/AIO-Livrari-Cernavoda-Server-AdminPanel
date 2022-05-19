using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using OShop.UI.ApiAuth;
using OShop.UI.ApiAuthManage;
using System;

namespace OShop.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OnlineShopDbContext>(options =>
            {
                options.UseSqlServer(Configuration
                    .GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction => sqlServerOptionsAction.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)
                    );
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                    .AddEntityFrameworkStores<OnlineShopDbContext>()
                    .AddDefaultTokenProviders();
            /* services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7);
                options.Cookie.Name = Guid.NewGuid().ToString();
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }); */
            services.AddAntiforgery(options =>
            {
                options.Cookie.Expiration = TimeSpan.FromMinutes(60);
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Auth/Login";
                options.LogoutPath = $"/Auth/Logout";
                options.AccessDeniedPath = $"/Error";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;
                options.Validate();
            });
            services.AddTransient<IFileManager, FileManager>();
            services.AddControllersWithViews();

            services.AddRazorPages();
            services.AddMvc();
            services.AddTransient<ValidateBearerToken>();
            services.AddTransient<ValidateBearerTokenManage>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 7;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id}");
                endpoints.MapRazorPages();
            });
        }
    }
}
