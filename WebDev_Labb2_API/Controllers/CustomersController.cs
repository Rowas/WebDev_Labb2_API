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

                using (var db = new DBContext())
                {
                    List<Customers> result = new();

                    result = db.Customers.ToList();

                    return result;
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpGet("{email}", Name = "GetCustomer")]
        public Customers Get(string email)
        {
            try
            {
                using (var db = new DBContext())
                {
                    Customers result = new();

                    result = db.Customers.Where(c => c.email == email).FirstOrDefault();

                    return result;
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
