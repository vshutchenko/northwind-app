using Newtonsoft.Json;
using Northwind.Services.Products;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace NorthwindMvcApp
{
    public class ApiClient
    {
        //private WeakReference<List<Product>> productsCache;
        //private HttpClient client;

        //public async IAsyncEnumerable<Product> GetAllProducts()
        //{
        //    int offset = 0;
        //    int limit = 10;

        //    if (!productsCache.TryGetTarget(out var products))
        //    {
        //        var json = await this.client.GetStringAsync($"api/products?offset=0&limit={int.MaxValue}");
        //        products = JsonConvert.DeserializeObject<List<Product>>(json);
        //        productsCache.SetTarget(products);
        //    }

        //    return products.As;
        //}

        //public IEnumerable<Product> GetAllCategories()
        //{

        //}

        //public IEnumerable<Product> GetAllEmployees()
        //{

        //}

        //public IEnumerable<Product> GetAllProducts()
        //{

        //}

        //public IEnumerable<Product> GetAllProducts()
        //{

        //}
    }
}
