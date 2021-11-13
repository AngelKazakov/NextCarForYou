using CarSalesSystem.Data;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Services;
using CarSalesSystem.Services.Advertisement;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Colors;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Schema;

namespace CarSalesSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        => Configuration = configuration;


        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarSalesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;

                })
                .AddEntityFrameworkStores<CarSalesDbContext>();

            services
                .AddControllersWithViews(options =>
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IModelService, ModelService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<ITechnicalService, TechnicalService>();
            services.AddTransient<IAdvertisementService, AdvertisementService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
