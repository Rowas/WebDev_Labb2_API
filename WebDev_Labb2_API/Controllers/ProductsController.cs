using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;
using WebDev_Labb2_API.Repository;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productRepository;

        public ProductsController(IProductsRepository productsRepository)
        {
            _productRepository = productsRepository;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<ActionResult<IEnumerable<Products>>> Get()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpGet("{sku_or_name}", Name = "GetProductBySkuOrName")]
        public async Task<ActionResult<Products>> Get(string sku_or_name)
        {
            try
            {
                Products product;
                if (int.TryParse(sku_or_name, out int sku))
                {
                    product = await _productRepository.GetBySkuAsync(sku);
                }
                else
                {
                    product = await _productRepository.GetByNameAsync(sku_or_name);
                }

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpGet("category/{category}", Name = "GetProductsByCategory")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProductsByCategory(string category)
        {
            try
            {
                var products = await _productRepository.GetProductsByCategory(category);
                if (products == null || !products.Any())
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpPost(Name = "AddProduct")]
        public async Task<ActionResult<Products>> Post(Products product)
        {
            try
            {
                var existingProduct = await _productRepository.GetBySkuAsync(product.sku);
                if (existingProduct != null)
                {
                    return BadRequest(new { message = "Product already exists." });
                }

                var addedProduct = await _productRepository.AddAsync(product);
                return CreatedAtAction(nameof(Get), new { sku_or_name = addedProduct.sku }, addedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpDelete(Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(int sku)
        {
            try
            {
                var product = await _productRepository.GetBySkuAsync(sku);
                if (product == null)
                {
                    return NotFound();
                }
                await _productRepository.DeleteAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpPut("single", Name = "UpdateProduct")]
        public async Task<IActionResult> Put(Products product)
        {
            try
            {
                var updatedProduct = await _productRepository.UpdateProductAsync(product);
                if (updatedProduct == null)
                {
                    return NotFound();
                }

                return Ok(new { message = "Success", product = updatedProduct });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpPut("bulk", Name = "MassUpdateProducts")]
        public async Task<IActionResult> Put(IEnumerable<Products> products)
        {
            try
            {
                foreach (var product in products)
                {
                    var updatedProduct = await _productRepository.UpdateProductAsync(product);
                }
                return Ok(new { message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }
    }
}