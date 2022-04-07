using Northwind.Services.Products;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace NorthwindMvcApp
{
    public class ApiClient
    {
        private HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        public async Task<Product> GetProductAsync(int id)
        {
            var response = await this.httpClient.GetAsync($"api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var product = await stream.DeserializeAsync<Product>();

                return product;
            }

            return null;
        }

        public async IAsyncEnumerable<Product> GetProductsAsync(int offset = 0, int limit = int.MaxValue)
        {
            var response = await this.httpClient.GetAsync($"api/products?offset={offset}&limit={limit}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var products = stream.DeserializeAsyncEnumerable<Product>();

                await foreach (var product in products)
                {
                    yield return product;
                }
            }

            AsyncEnumerable.Empty<Product>();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await this.httpClient.DeleteAsync($"api/products/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            var json = await product.SerializeAsync();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PutAsync($"api/products/{id}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            var json = await product.SerializeAsync();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync($"api/products", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<ProductCategory> GetCategoryAsync(int id)
        {
            var response = await this.httpClient.GetAsync($"api/categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var category = await stream.DeserializeAsync<ProductCategory>();

                return category;
            }

            return null;
        }

        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset = 0, int limit = int.MaxValue)
        {
            var response = await this.httpClient.GetAsync($"api/categories?offset={offset}&limit={limit}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var categories = stream.DeserializeAsyncEnumerable<ProductCategory>();

                await foreach (var c in categories)
                {
                    yield return c;
                }
            }

            AsyncEnumerable.Empty<ProductCategory>();
        }
    }
}
