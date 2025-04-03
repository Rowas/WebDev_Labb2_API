using Microsoft.EntityFrameworkCore;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public class OrdersRepository : Repository<Orders>, IOrdersRepository
    {
        private readonly OrderMethods _orderMethods;

        public OrdersRepository(DBContext context) : base(context)
        {
            _orderMethods = new OrderMethods();
        }

        public override async Task<IEnumerable<Orders>> GetAllAsync()
        {
            return await _dbSet
                .OrderByDescending(o => o.order_date)
                .ToListAsync();
        }
        public async Task<IEnumerable<Orders>> GetAllOrdersByStatusAsync(string status)
        {
            return await _dbSet
                .Where(o => o.status == status)
                .OrderByDescending(o => o.order_date)
                .ToListAsync();
        }
        public async Task<Orders> GetByOrderIdAsync(string orderId)
        {
            return await _dbSet.FirstOrDefaultAsync(o => o.order_id == orderId);
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUserAsync(string username)
        {
            return await _dbSet
                .Where(o => o.username == username)
                .OrderByDescending(o => o.order_date)
                .ToListAsync();
        }

        public async Task<Orders> CreateOrderAsync(Orders order)
        {
            var newOrder = _orderMethods.CreateOrder(order);

            await _dbSet.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return newOrder;
        }

        public async Task<Orders> UpdateOrderStatusAsync(string orderId, string newStatus)
        {
            var order = await GetByOrderIdAsync(orderId);
            if (order == null)
                return null;

            order.status = newStatus;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}