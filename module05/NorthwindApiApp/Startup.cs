using System.Data.SqlClient;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Northwind.Services.EntityFrameworkCore;
using Northwind.Services.Products;
using Northwind.DataAccess;
using Northwind.DataAccess.SqlServer;
using Northwind.Services.DataAccess.Services;
using Northwind.Services.EntityFrameworkCore.Services;
using Northwind.Services.Employees;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

#pragma warning disable CA1822 // Mark members as static
namespace NorthwindApiApp
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configuring services.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if(this.Configuration["ServiceType"] == "InMemory")
            {
                services
               .AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthwindApiApp", Version = "v1" }))
               .AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("Northwind"))
               .AddScoped<IEmployeeManagementService, EmployeeManagementService>()
               .AddScoped<IProductManagementService, ProductManagementService>()
               .AddScoped<IProductCategoryManagementService, ProductCategoryManagementService>()
               .AddScoped<IProductCategoryPicturesService>(provider =>
                   new ProductCategoryPicturesService(
                       provider.GetService<NorthwindContext>(),
                       int.Parse(this.Configuration["PictureService:MaxFileSize"], CultureInfo.InvariantCulture)))
               .AddScoped(provider => new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper())
               .AddControllers();
                
            }
            else if(this.Configuration["ServiceType"] == "Northwind")
            {
                 services
                .AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthwindApiApp", Version = "v1" }))
                .AddScoped<NorthwindDataAccessFactory, SqlServerDataAccessFactory>()
                .AddScoped<IEmployeeManagementService, EmployeeManagementDataAccessService>()
                .AddScoped<IProductManagementService, ProductManagementDataAccessService>()
                .AddScoped<IProductCategoryManagementService, ProductCategoriesManagementDataAccessService>()
                .AddScoped<IProductCategoryPicturesService>(provider =>
                    new ProductCategoryPicturesManagementDataAccessService(
                        provider.GetService<NorthwindDataAccessFactory>(),
                        int.Parse(this.Configuration["PictureService:MaxFileSize"], CultureInfo.InvariantCulture)))
                .AddScoped(provider => new SqlConnection(this.Configuration.GetConnectionString("SqlService")))
                .AddScoped(provider => new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper())
                .AddControllers();
            }
            
        }

        /// <summary>
        /// Configuring middlewares.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
