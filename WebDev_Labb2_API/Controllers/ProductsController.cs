﻿using Microsoft.AspNetCore.Mvc;
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
                List<Products> products = new();

                using (var db = new DBContext())
                {
                    var result = db.Products.FirstOrDefault();

                    products.Add(result);
                }

                return products;
            }
            catch (Exception e)
            {
                Console.Write("Error");
                return null;
            }
        }
    }
}
