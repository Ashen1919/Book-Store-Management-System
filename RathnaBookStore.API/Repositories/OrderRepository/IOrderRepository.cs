using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetOrdersAsync(DateTime? filterStartDate = null, DateTime? filterEndDate = null,
            string? sortBy = null, bool isAcsending = true, int pageNumber = 1, int pageSize = 1000);

        Task<Order?> GetOrdersByIdAsync(Guid id);

        Task<Order?> UpdateOrderAsync(Guid id,  Order order);

    }
}
