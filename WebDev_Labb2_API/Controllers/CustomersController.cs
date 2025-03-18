using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        [HttpGet(Name = "GetCustomers")]
        public List<Customers> Get()
        {
            try
            {
                List<Customers> customers = new();

                using (var db = new DBContext())
                {
                    var result = db.Customers.Find();

                    customers.Add(result);
                }

                return customers;
            }
            catch (Exception e)
            {
                Console.Write("Error");
                return null;
            }
        }
    }
}
