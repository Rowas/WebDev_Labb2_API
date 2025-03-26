using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        [HttpGet(Name = "GetProducts")]
        public List<Products> Get()
        {
            try
            {
                using (var db = new DBContext())
                {
                    List<Products> result = new();

                    result = db.Products.OrderBy(n => n.number).ToList();

                    return result;
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpGet("{sku_or_name}", Name = "GetProductBySkuOrName")]
        public Products Get(string sku_or_name)
        {
            try
            {
                using (var db = new DBContext())
                {
                    int sku;
                    if (int.TryParse(sku_or_name.ToString(), out sku) == true)
                    {
                        var result = db.Products.Where(p => p.sku == sku).FirstOrDefault();
                        return result;
                    }
                    else if (int.TryParse(sku_or_name.ToString(), out sku) == false)
                    {
                        string name = sku_or_name.ToString();
                        var result = db.Products.Where(p => p.name == name).FirstOrDefault();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpPatch("{ProdToUpdate}", Name = "UpdateProduct")]
        public IActionResult Patch(string ProdToUpdate, Products productToUpdate)
        {
            try
            {
                var ProdMethods = new ProdMethods();
                using (var db = new DBContext())
                {
                    int sku;
                    if (int.TryParse(ProdToUpdate.ToString(), out sku) == true)
                    {
                        Products product = db.Products.Where(p => p.sku == sku).FirstOrDefault();
                        if (product == null)
                        {
                            return BadRequest(new { message = $"Product not found." });
                        }
                        else
                        {
                            product = ProdMethods.AssignProductValues(product, productToUpdate);
                            db.SaveChanges();
                            return Ok(new { message = "Success", product });
                        }
                    }
                    else if (int.TryParse(ProdToUpdate.ToString(), out var name) == false)
                    {
                        Products product = db.Products.Where(p => p.name == productToUpdate.name).FirstOrDefault();
                        if (product == null)
                        {
                            return BadRequest(new { message = $"Product not found." });
                        }
                        else
                        {
                            product = ProdMethods.AssignProductValues(product, productToUpdate);
                            db.SaveChanges();
                            return Ok(new { message = "Success", product });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = $"Product not found." });
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error");
                return null;
            }
        }

        [HttpPost(Name = "AddProduct")]
        public IActionResult Post(Products receivedProduct)
        {
            var newProd = receivedProduct;
            try
            {
                using (var db = new DBContext())
                {
                    if (db.Products.Where(p => p.sku == newProd.sku).FirstOrDefault() != null)
                    {
                        return BadRequest(new { message = "Product already exists." });
                    }
                    db.Products.Add(receivedProduct);
                    db.SaveChanges();
                    return Ok(new { message = "Success", receivedProduct });
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpDelete("{sku}", Name = "DeleteProduct")]
        public IActionResult Delete(int sku)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var product = db.Products.Where(p => p.sku == sku).FirstOrDefault();
                    if (product != null)
                    {
                        return BadRequest(new { message = "Product not found." });
                    }
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return Ok(new { message = "Success", product });
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }
    }
}
