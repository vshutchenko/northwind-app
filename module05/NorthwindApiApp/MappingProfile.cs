using AutoMapper;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.Products;
using Northwind.Services.Employees;
using Northwind.Services.Products;

namespace NorthwindApiApp
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<Employee, EmployeeTransferObject>();
            this.CreateMap<EmployeeTransferObject, Employee>();
            this.CreateMap<Product, ProductTransferObject>();
            this.CreateMap<ProductTransferObject, Product>();
            this.CreateMap<ProductCategory, ProductCategoryTransferObject>();
            this.CreateMap<ProductCategoryTransferObject, ProductCategory>();
        }
    }
}
