using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public interface IOrdersRepository : IRepository<Orders>
    {
        Task<IEnumerable<Orders>> GetOrdersByUserAsync(string username);
        Task<Orders> GetByOrderIdAsync(string orderId);
        Task<IEnumerable<Orders>> GetAllOrdersByStatusAsync(string status);
        Task<Orders> CreateOrderAsync(Orders order);
        Task<Orders> UpdateOrderStatusAsync(string orderId, string newStatus);
    }
}
