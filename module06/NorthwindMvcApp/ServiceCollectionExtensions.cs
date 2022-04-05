using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Services.Products;
using Northwind.DataAccess;
using Northwind.DataAccess.SqlServer;
using Northwind.Services.DataAccess.Services;
using Northwind.Services.EntityFrameworkCore.Services;
using Northwind.Services.Employees;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Blogging;
using Northwind.Services.Blogging;
using System;
using NorthwindMvcApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Northwind.Services.Customers;

namespace NorthwindMvcApp
{
    /// <summary>
    /// Extension methods for sevice collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add northwind services to service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        public static void UseNorthwindServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services
               .AddScoped<NorthwindDataAccessFactory, SqlServerDataAccessFactory>()
               .AddScoped<IBloggingService, BloggingService>()
               .AddScoped<ICustomerManagementService, CustomerManagementDataAccessService>()
               .AddScoped<IEmployeeManagementService, EmployeeManagementDataAccessService>()
               .AddScoped<IProductManagementService, ProductManagementDataAccessService>()
               .AddScoped<IProductCategoryManagementService, ProductCategoriesManagementDataAccessService>()
               .AddScoped<IProductCategoryPicturesService>(provider =>
                   new ProductCategoryPicturesManagementDataAccessService(
                       provider.GetService<NorthwindDataAccessFactory>(),
                       int.Parse(configuration["PictureService:MaxFileSize"], CultureInfo.InvariantCulture)))
               .AddScoped(provider => new SqlConnection(configuration.GetConnectionString("SqlService")))
               .AddScoped(provider => new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper())
               .AddDbContext<BloggingContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("SQLCONNSTR_NORTHWIND_BLOGGING")))
               .AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("UsersDb")))
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
        }

        /// <summary>
        /// Add inmemory services to service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        public static void UseInMemoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services
               .AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("Northwind"))
               .AddScoped<IBloggingService, BloggingService>()
               .AddScoped<ICustomerManagementService, CustomerManagementService>()
               .AddScoped<IEmployeeManagementService, EmployeeManagementService>()
               .AddScoped<IProductManagementService, ProductManagementService>()
               .AddScoped<IProductCategoryManagementService, ProductCategoryManagementService>()
               .AddScoped<IProductCategoryPicturesService>(provider =>
                   new ProductCategoryPicturesService(
                       provider.GetService<NorthwindContext>(),
                       int.Parse(configuration["PictureService:MaxFileSize"], CultureInfo.InvariantCulture)))
               .AddScoped(provider => new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper())
               .AddDbContext<BloggingContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("SQLCONNSTR_NORTHWIND_BLOGGING")))
               .AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("UsersDb")))
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
        }
    }
}
