using AutoMapper;
using Microsoft.AspNetCore.Http;
using Northwind.DataAccess.Customers;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.Products;
using Northwind.Services.Blogging;
using Northwind.Services.Customers;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Article;
using NorthwindMvcApp.ViewModels.Category;
using NorthwindMvcApp.ViewModels.Product;
using System.IO;

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
            this.CreateMap<BlogArticle, BlogArticleInputViewModel>().ForMember(x => x.AllProducts, opt => opt.Ignore()).ReverseMap();


            this.CreateMap<Product, ProductViewModel>().ReverseMap();
            this.CreateMap<ProductInputViewModel, Product>().ForSourceMember(x => x.CategoryItems, opt => opt.DoNotValidate()).ReverseMap();

            this.CreateMap<BlogArticle, BlogArticleViewModel>().ReverseMap();
            this.CreateMap<BlogComment, BlogCommentViewModel>().ReverseMap();
            this.CreateMap<BlogArticleProduct, ProductViewModel>().ReverseMap();


            this.CreateMap<ProductCategory, CategoryViewModel>();
            this.CreateMap<CategoryInputViewModel, ProductCategory>().ForSourceMember(x => x.NewPicture, opt => opt.DoNotValidate()).ReverseMap();

            this.CreateMap<Customer, CustomerTransferObject>().ReverseMap();
            this.CreateMap<Employee, EmployeeTransferObject>().ReverseMap();
            this.CreateMap<Product, ProductTransferObject>().ReverseMap();
            this.CreateMap<ProductCategory, ProductCategoryTransferObject>().ReverseMap();

            this.CreateMap<Customer, CustomerEntity>().ReverseMap();
            this.CreateMap<Employee, EmployeeEntity>().ReverseMap();
            this.CreateMap<Product, ProductEntity>().ReverseMap();
            this.CreateMap<ProductCategory, CategoryEntity>().ReverseMap();

            this.CreateMap<BlogArticle, BlogArticleEntity>().ReverseMap();
            this.CreateMap<BlogArticleProduct, BlogArticleProductEntity>().ReverseMap();
            this.CreateMap<BlogComment, BlogCommentEntity>().ReverseMap();
        }
    }
}
