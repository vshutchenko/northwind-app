using AutoMapper;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.Products;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;
using Northwind.Services.EntityFrameworkCore.Entities;
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

            this.CreateMap<Employee, EmployeesEntity>();
            this.CreateMap<EmployeesEntity, Employee>();
            this.CreateMap<Product, ProductsEntity>();
            this.CreateMap<ProductsEntity, Product>();
            this.CreateMap<ProductCategory, CategoriesEntity>();
            this.CreateMap<CategoriesEntity, ProductCategory>();

            this.CreateMap<BlogArticle, BlogArticleEntity>();
            this.CreateMap<BlogArticleEntity, BlogArticle>();

            this.CreateMap<BlogArticleProduct, BlogArticleProductEntity>();
            this.CreateMap<BlogArticleProductEntity, BlogArticleProduct>();

            this.CreateMap<BlogComment, BlogCommentEntity>();
            this.CreateMap<BlogCommentEntity, BlogComment>();
        }
    }
}
