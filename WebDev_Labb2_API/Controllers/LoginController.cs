using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private CustomerMethods CustomerMethods = new CustomerMethods();
        private GetJWT GetJWT = new GetJWT();

        [HttpPost(Name = "LoginCustomer")]
        public IActionResult Post(Login credentials)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var customer = db.Customers.Where(c => c.email == credentials.email).FirstOrDefault();
                    if (customer == null)
                    {
                        return BadRequest(new { message = "Customer not found or Password Wrong" });
                    }
                    var testResult = CustomerMethods.VerifyLogin(customer, credentials.password);

                    if (testResult.ToString() == "Success")
                    {
                        var token = GetJWT.GenerateJwtToken(customer.username, customer.userlevel);
                        return Ok(new { message = "Success", token });
                    }
                    else
                    {
                        return Forbid();
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
