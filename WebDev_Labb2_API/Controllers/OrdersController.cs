using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        [HttpGet(Name = "GetOrders")]
        public List<Orders> Get()
        {
            try
            {
                using (var db = new DBContext())
                {
                    List<Orders> result = new();

                    result = db.Orders.OrderBy(n => n.order_date).ToList();

                    return result;
                }
            }
            catch
            {
                Console.Write("Error");
                return null;
            }
        }
        [HttpGet("{order_id}", Name = "GetSpecificOrder")]
        public Orders Get(string order_id)
        {
            try
            {
                using (var db = new DBContext())
                {
                    Orders result = new();
                    result = db.Orders.Where(o => o.order_id == order_id).FirstOrDefault();
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
        [HttpPost(Name = "NewOrder")]
        public IActionResult Post(Orders receivedOrder)
        {
            try
            {
                using (var db = new DBContext())
                {
                    db.Orders.Add(receivedOrder);
                    db.SaveChanges();
                    return Ok(new { message = "Success", receivedOrder });
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
