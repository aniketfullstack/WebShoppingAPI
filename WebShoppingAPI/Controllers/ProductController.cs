using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Data;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Controllers
{
    public class ProductController : BaseAPIController
    {
        private readonly IProductRepository _productRepository;
        private readonly DatabaseContext _databaseContext;
        public ProductController(IProductRepository productRepository, DatabaseContext databaseContext)
        {
            _productRepository = productRepository;
            _databaseContext = databaseContext;
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var i = await _productRepository.CreateProductAsync(product);
            if (i > 0)
            {
                return Ok("Product added successfully");
            }
            else
            {
                return BadRequest("Unable to create product");
            }
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            try
            {
                int m = await _productRepository.UpdateProductAsync(product);
                if (m > 0)
                {
                    return Ok("Product updated successfully");
                }
                else
                {
                    return BadRequest("Product update failed");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            int d = await _productRepository.DeleteProductAsync(id);
            if (d > 0)
            {
                return Ok("Product deleted successfully");
            }
            else
            {
                return BadRequest("Product delete failed");
            }
        }


        [HttpGet]
        [Route("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("GetProductById")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        private bool ProductExists(int id)
        {
            return _databaseContext.Product.Any(e => e.Id == id);
        }

    }
}
