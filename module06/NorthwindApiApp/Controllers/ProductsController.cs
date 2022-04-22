using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Products controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManagementService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="service">Service to retrieve products.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductsController(IProductManagementService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Gets products.
        /// </summary>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>Products collection.</returns>
        // GET: api/<ProductsController>
        [HttpGet]
        public async IAsyncEnumerable<Product> GetProductsAsync(int offset = 0, int limit = 10)
        {
            await foreach (var product in this.service.GetProductsAsync(offset, limit))
            {
                yield return product;
            }
        }

        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Product if it exists; otherwise false.</returns>
        // GET api/<ProductsController>/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            Product product = await this.service.GetProductAsync(id);
            if (product != null)
            {
                return this.Ok(product);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Creates product.
        /// </summary>
        /// <param name="product">Product to create.</param>
        /// <returns>Created product.</returns>
        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProductAsync(Product product)
        {
            if (product is null)
            {
                return this.BadRequest();
            }

            int id = await this.service.CreateProductAsync(product);
            if (id > 0)
            {
                return this.CreatedAtAction(nameof(PostProductAsync), new { Id = id }, product);
            }
            else
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Updates product.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="product">Product to update.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // PUT api/<ProductsController>/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductAsync(int id, Product product)
        {
            if (product is null || id != product.Id)
            {
                return this.BadRequest();
            }

            if (await this.service.UpdateProductAsync(id, product))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes product.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // DELETE api/<ProductsController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            if (await this.service.DestroyProductAsync(id))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Gets products count.
        /// </summary>
        /// <returns>Products count.</returns>
        // GET api/<ProductsController>/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetProductsCountAsync()
        {
            return this.Ok(await this.service.CountAsync());
        }
    }
}
