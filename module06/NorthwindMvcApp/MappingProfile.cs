using AutoMapper;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.Products;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels.Category;
using NorthwindMvcApp.ViewModels.Product;

namespace NorthwindMvcApp
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
            this.CreateMap<Product, ProductViewModel>();
            this.CreateMap<ProductCategory, CategoryViewModel>();

            this.CreateMap<Employee, EmployeeTransferObject>().ReverseMap();
            this.CreateMap<Product, ProductTransferObject>().ReverseMap();
            this.CreateMap<ProductCategory, ProductCategoryTransferObject>().ReverseMap();

            this.CreateMap<Employee, EmployeeEntity>().ReverseMap();
            this.CreateMap<Product, ProductEntity>().ReverseMap();
            this.CreateMap<ProductCategory, CategoryEntity>().ReverseMap();

            this.CreateMap<BlogArticle, BlogArticleEntity>().ReverseMap();
            this.CreateMap<BlogArticleProduct, BlogArticleProductEntity>().ReverseMap();
            this.CreateMap<BlogComment, BlogCommentEntity>().ReverseMap();
        }
    }
}
