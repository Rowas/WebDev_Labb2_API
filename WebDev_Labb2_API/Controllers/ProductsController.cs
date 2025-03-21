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

        [HttpPatch("{sku}", Name = "UpdateProductStockStatus")]
        public string Patch(int sku, bool? in_stock, string? field, string? value)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var product = db.Products.Where(p => p.sku == sku).FirstOrDefault();
                    if (product == null)
                    {
                        return "Product not found.";
                    }
                    if (in_stock != null)
                    {
                        product.in_stock = in_stock.Value;
                        if (in_stock == false)
                        {
                            db.SaveChanges();
                            return $"Product stock status updated: {product.name} now out of stock";
                        }
                        db.SaveChanges();
                        return $"Product stock status updated: {product.name} now in stock";
                    }
                    if (field != null && value != null)
                    {
                        switch (field)
                        {
                            case "price":
                                product.price = double.Parse(value);
                                break;
                            case "name":
                                product.name = value;
                                break;
                            case "appearance":
                                product.appearance = value;
                                break;
                            case "atomic_mass":
                                product.atomic_mass = double.Parse(value);
                                break;
                            case "category":
                                product.category = value;
                                break;
                            case "density":
                                product.density = double.Parse(value);
                                break;
                            case "melt":
                                product.melt = double.Parse(value);
                                break;
                            case "boil":
                                product.boil = double.Parse(value);
                                break;
                            case "number":
                                product.number = double.Parse(value);
                                break;
                            case "phase":
                                product.phase = value;
                                break;
                            case "source":
                                product.source = value;
                                break;
                            case "bohr_model_image":
                                product.bohr_model_image = value;
                                break;
                            case "summary":
                                product.summary = value;
                                break;
                            case "symbol":
                                product.symbol = value;
                                break;
                            case "cpk_hex":
                                product.cpk_hex = value;
                                break;
                            case "block":
                                product.block = value;
                                break;
                            default:
                                return "Field not found. No changes made.";
                        }
                    }
                    db.SaveChanges();
                    return $"Product field: {field} have been updated to {value} sucessfully.";
                }

            }
            catch
            {
                Console.Write("Error");
                return null;
            }
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
                    Products product = new Products
                    {
                        sku = newProd.sku,
                        price = newProd.price,
                        in_stock = newProd.in_stock,
                        name = newProd.name,
                        appearance = newProd.appearance,
                        atomic_mass = newProd.atomic_mass,
                        category = newProd.category,
                        density = newProd.density,
                        melt = newProd.melt,
                        boil = newProd.boil,
                        number = newProd.number,
                        phase = newProd.phase,
                        source = newProd.source,
                        bohr_model_image = newProd.bohr_model_image,
                        summary = newProd.summary,
                        symbol = newProd.symbol,
                        cpk_hex = newProd.cpk_hex,
                        block = newProd.block
                    };
                    db.Products.Add(product);
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
