﻿using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public interface IOrdersRepository : IRepository<Orders>
    {
        Task<IEnumerable<Orders>> GetOrdersByUserAsync(string username);
        Task<Orders> GetByOrderIdAsync(string orderId);
        Task<IEnumerable<Orders>> GetOrdersByDateRangeAsync(DateOnly startDate, DateOnly endDate);
    }
}
