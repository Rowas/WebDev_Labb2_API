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

        //[HttpPatch("{name_or_sku}", Name = "UpdateProductStockStatus")]
        //public string Patch(string name_or_sku, bool? in_stock)
        //{
        //    try
        //    {
        //        using (var db = new DBContext())
        //        {
        //            if (int.TryParse(name_or_sku.ToString(), out var sku) == true)
        //            {
        //                var product = db.Products.Where(p => p.sku == sku).FirstOrDefault();

        //                if (product == null)
        //                {
        //                    return $"{name_or_sku} not found.";
        //                }
        //                else
        //                {
        //                    product.in_stock = in_stock.Value;
        //                    db.SaveChanges();
        //                    return $"Product stock status updated: {product.name} stock status: {product.in_stock}";
        //                }
        //            }
        //            else if (int.TryParse(name_or_sku.ToString(), out var name) == false)
        //            {
        //                var product = db.Products.Where(p => p.name == name_or_sku).FirstOrDefault();
        //                if (product == null)
        //                {
        //                    return $"{name_or_sku} not found.";
        //                }
        //                else
        //                {
        //                    product.in_stock = in_stock.Value;
        //                    db.SaveChanges();
        //                    return $"Product stock status updated: {product.name} stock status: {product.in_stock}";
        //                }
        //            }
        //            else
        //            {
        //                return $"{name_or_sku} not found. No changes made";
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        Console.Write("Error");
        //        return null;
        //    }
        //}

        [HttpPatch("{ProdToUpdate}", Name = "UpdateProduct")]
        public string Patch(string ProdToUpdate, Products product)
        {
            try
            {
                using (var db = new DBContext())
                {
                    int sku;
                    if (int.TryParse(ProdToUpdate.ToString(), out sku) == true)
                    {
                        var productToUpdate = db.Products.Where(p => p.sku == sku).FirstOrDefault();
                        if (productToUpdate == null)
                        {
                            return $"{ProdToUpdate} not found.";
                        }
                        else
                        {
                            db.Products.Update(product);
                            productToUpdate = AssignProductValues(product);
                            db.SaveChanges();
                            return $"Product updated: {productToUpdate.name}";
                        }
                    }
                    else if (int.TryParse(ProdToUpdate.ToString(), out var name) == false)
                    {
                        var productToUpdate = db.Products.Where(p => p.name == ProdToUpdate).FirstOrDefault();
                        if (productToUpdate == null)
                        {
                            return $"{ProdToUpdate} not found.";
                        }
                        else
                        {
                            productToUpdate = AssignProductValues(product);
                            db.SaveChanges();
                            return $"Product updated: {productToUpdate.name}";
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

        private Products AssignProductValues(Products product)
        {
            Products productToUpdate = new();
            productToUpdate.price = product.price;
            productToUpdate.in_stock = product.in_stock;
            productToUpdate.name = product.name;
            productToUpdate.appearance = product.appearance;
            productToUpdate.atomic_mass = product.atomic_mass;
            productToUpdate.category = product.category;
            productToUpdate.density = product.density;
            productToUpdate.melt = product.melt;
            productToUpdate.boil = product.boil;
            productToUpdate.number = product.number;
            productToUpdate.phase = product.phase;
            productToUpdate.source = product.source;
            productToUpdate.bohr_model_image = product.bohr_model_image;
            productToUpdate.summary = product.summary;
            productToUpdate.symbol = product.symbol;
            productToUpdate.cpk_hex = product.cpk_hex;
            productToUpdate.block = product.block;
            return productToUpdate;
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
    }
}
