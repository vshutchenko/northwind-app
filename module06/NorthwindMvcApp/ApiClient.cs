using Newtonsoft.Json;
using Northwind.Services.Products;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindMvcApp
{
    public class ApiClient
    {
        //private WeakReference<List<Product>> productsCache;
        //private WeakReference<List<Product>> categoriesCache;
        //private WeakReference<List<Product>> productsCache;
        //private WeakReference<List<Product>> productsCache;
        //private WeakReference<List<Product>> productsCache;
        //private WeakReference<List<Product>> relatedProductsCache;
        //private HttpClient client;

        //public async Task<IEnumerable<Product>> GetProducts()
        //{
        //    if (!productsCache.TryGetTarget(out var products))
        //    {
        //        var json = await this.client.GetStringAsync($"api/products?offset=0&limit={int.MaxValue}");
        //        products = JsonConvert.DeserializeObject<List<Product>>(json);
        //        productsCache.SetTarget(products);
        //    }

        //    return products;
        //}

        //public IEnumerable<Product> GetCategories()
        //{

        //}

        //public IEnumerable<Product> GetEmployees()
        //{

        //}

        //public IEnumerable<Product> GetArticles()
        //{

        //}

        //public IEnumerable<Product> GetComments(int articleId)
        //{

        //}

        //public IEnumerable<Product> GetRelatedProducts(int articleId)
        //{

        //}
    }
}
