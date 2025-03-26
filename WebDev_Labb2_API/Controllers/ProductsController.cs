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
        public string Patch(string ProdToUpdate, Products productToUpdate)
        {
            try
            {
                using (var db = new DBContext())
                {
                    int sku;
                    if (int.TryParse(ProdToUpdate.ToString(), out sku) == true)
                    {
                        Products product = db.Products.Where(p => p.sku == sku).FirstOrDefault();
                        if (product == null)
                        {
                            return $"{productToUpdate} not found.";
                        }
                        else
                        {
                            product = AssignProductValues(product, productToUpdate);
                            db.SaveChanges();
                            return $"Product updated: {product.name}";
                        }
                    }
                    else if (int.TryParse(ProdToUpdate.ToString(), out var name) == false)
                    {
                        Products product = db.Products.Where(p => p.name == productToUpdate.name).FirstOrDefault();
                        if (product == null)
                        {
                            return $"{productToUpdate} not found.";
                        }
                        else
                        {
                            product = AssignProductValues(product, productToUpdate);
                            db.SaveChanges();
                            return $"Product updated: {product.name}";
                        }
                    }
                    else
                    {
                        return "Product not found. No changes made.";
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error");
                return null;
            }
        }

        private Products AssignProductValues(Products oldProduct, Products newProduct)
        {
            oldProduct.price = newProduct.price;
            oldProduct.in_stock = newProduct.in_stock;
            oldProduct.name = newProduct.name;
            oldProduct.appearance = newProduct.appearance;
            oldProduct.atomic_mass = newProduct.atomic_mass;
            oldProduct.category = newProduct.category;
            oldProduct.density = newProduct.density;
            oldProduct.melt = newProduct.melt;
            oldProduct.boil = newProduct.boil;
            oldProduct.number = newProduct.number;
            oldProduct.phase = newProduct.phase;
            oldProduct.source = newProduct.source;
            oldProduct.bohr_model_image = newProduct.bohr_model_image;
            oldProduct.summary = newProduct.summary;
            oldProduct.symbol = newProduct.symbol;
            oldProduct.cpk_hex = newProduct.cpk_hex;
            oldProduct.block = newProduct.block;
            return oldProduct;
        }

        [HttpPost(Name = "AddProduct")]
        public string Post(Products receivedProduct)
        {
            var newProd = receivedProduct;
            try
            {
                using (var db = new DBContext())
                {
                    if (db.Products.Where(p => p.sku == newProd.sku).FirstOrDefault() != null)
                    {
                        return "Product already exists.";
                    }
                    db.Products.Add(receivedProduct);
                    db.SaveChanges();
                    return "Product added.";
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpDelete("{sku}", Name = "DeleteProduct")]
        public string Delete(int sku)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var product = db.Products.Where(p => p.sku == sku).FirstOrDefault();
                    if (product != null)
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        return $"Product {product.name} removed.";
                    }
                    return "Product not found.";
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
