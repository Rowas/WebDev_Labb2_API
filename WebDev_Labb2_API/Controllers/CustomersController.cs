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

                    if (result == null)
                    {
                        return null;
                    }

                    return result;
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpPost(Name = "AddCustomer")]
        public string Post(Customers receivedCustomer)
        {
            Customers newCustomer = receivedCustomer;
            try
            {
                using (var db = new DBContext())
                {
                    if (db.Customers.Where(c => c.email == newCustomer.email).FirstOrDefault() != null)
                    {
                        return "Customer already exists.";
                    }
                    if (db.Customers.Where(c => c.username == newCustomer.username).FirstOrDefault() != null)
                    {
                        return "Username already exists.";
                    }
                    var deliveryadress = new DeliveryAddress
                    {
                        street = newCustomer.delivery_adress.street,
                        post_code = newCustomer.delivery_adress.post_code,
                        city = newCustomer.delivery_adress.city,
                        country = newCustomer.delivery_adress.country
                    };
                    var customer = new Customers
                    {
                        username = newCustomer.username,
                        userlevel = "customer",
                        firstname = newCustomer.firstname,
                        lastname = newCustomer.lastname,
                        email = newCustomer.email,
                        mobile_number = newCustomer.mobile_number,
                        delivery_adress = deliveryadress
                    };
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return $"Customer {newCustomer.firstname} {newCustomer.lastname} added sucessfully.";
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpPatch("{email}", Name = "UpdateCustomer")]
        public string Patch(string email, Customers patchedCustomer)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var customer = db.Customers.Where(c => c.email == patchedCustomer.email).FirstOrDefault();
                    if (customer != null)
                    {
                        customer = patchedCustomer;
                        db.SaveChanges();

                        return $"Customer {customer.email} have been updated sucessfully.";
                    }

                    return "Customer not found";
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }

        [HttpDelete("{email}", Name = "DeleteCustomer")]
        public string Delete(string email)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var customer = db.Customers.Where(c => c.email == email).FirstOrDefault();
                    if (customer != null)
                    {
                        db.Customers.Remove(customer);
                        db.SaveChanges();
                        return $"Customer {customer.email} have been deleted sucessfully.";
                    }
                    return "Customer not found";
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
