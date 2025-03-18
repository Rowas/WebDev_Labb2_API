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

                    result = db.Products.ToList();

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
    }
}
