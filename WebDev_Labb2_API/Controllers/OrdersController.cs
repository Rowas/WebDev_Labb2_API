using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;
using WebDev_Labb2_API.Repository;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        [HttpGet(Name = "GetOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Orders>>> Get()
        {
            try
            {
                var orders = await _ordersRepository.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpGet("user/{username}", Name = "GetUserOrders")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Orders>>> GetUserOrders(string username)
        {
            try
            {
                if (User.Identity.Name != username && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var orders = await _ordersRepository.GetOrdersByUserAsync(username);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpGet("{orderId}", Name = "GetOrder")]
        [Authorize]
        public async Task<ActionResult<Orders>> Get(string orderId)
        {
            try
            {
                var order = await _ordersRepository.GetByOrderIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                if (User.Identity.Name != order.username && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpPost(Name = "CreateOrder")]
        [Authorize]
        public async Task<ActionResult<Orders>> Post(Orders order)
        {
            try
            {
                order.order_id = order.username + "_" + Guid.NewGuid().ToString("N").Substring(0, 8);
                order.status = "Pending";

                var createdOrder = await _ordersRepository.CreateOrderAsync(order);
                return CreatedAtAction(nameof(Get), new { orderId = createdOrder.order_id }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpPatch("{orderId}/status", Name = "UpdateOrderStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Orders>> UpdateStatus(string orderId, [FromBody] string newStatus)
        {
            try
            {
                var updatedOrder = await _ordersRepository.UpdateOrderStatusAsync(orderId, newStatus);
                if (updatedOrder == null)
                {
                    return NotFound();
                }

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }
    }
}
