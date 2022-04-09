using Northwind.Services.Products;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Northwind.Services.Employees;
using Northwind.Services.Customers;

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

                category.Picture = await this.GetCategoryPictureAsync(id);

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
                    c.Picture = await this.GetCategoryPictureAsync(c.Id);
                    yield return c;
                }
            }

            AsyncEnumerable.Empty<ProductCategory>();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var response = await this.httpClient.DeleteAsync($"api/categories/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(int id, ProductCategory category)
        {
            var json = await category.SerializeAsync();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PutAsync($"api/categories/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                var isUpdated = await this.UpdateCategoryPictureAsync(id, category.Picture);
                return isUpdated;
            }

            return false;
        }

        public async Task<(bool isCreated, int id)> CreateCategoryAsync(ProductCategory category)
        {
            var json = await category.SerializeAsync();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync($"api/categories", content);
            if (response.IsSuccessStatusCode)
            {
                var categoryStream = await response.Content.ReadAsStreamAsync();
                var id = (await categoryStream.DeserializeAsync<ProductCategory>()).Id;

                var isCreated = await this.UpdateCategoryPictureAsync(id, category.Picture);

                return (isCreated, id);
            }

            return (false, -1);
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            var response = await this.httpClient.GetAsync($"api/employees/{id}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var employee = await stream.DeserializeAsync<Employee>();

                return employee;
            }

            return null;
        }

        public async Task<Customer> GetCustomerAsync(string id)
        {
            var response = await this.httpClient.GetAsync($"api/customers/{id}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var customer = await stream.DeserializeAsync<Customer>();

                return customer;
            }

            return null;
        }

        private async Task<byte[]> GetCategoryPictureAsync(int id)
        {
            var response = await this.httpClient.GetAsync($"api/categories/{id}/picture");

            if (response.IsSuccessStatusCode)
            {
                var picture = await response.Content.ReadAsByteArrayAsync();

                return picture;
            }

            return Array.Empty<byte>();
        }

        private async Task<bool> UpdateCategoryPictureAsync(int id, byte[] picture)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(picture, 0, picture.Length), "categoryPicture", $"category{id}");

            var response = await this.httpClient.PutAsync($"api/categories/{id}/picture", form);

            return response.IsSuccessStatusCode;
        }
    }
}
