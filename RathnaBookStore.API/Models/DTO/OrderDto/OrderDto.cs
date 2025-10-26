using RathnaBookStore.API.Models.DTO.OrderItemDto;

namespace RathnaBookStore.API.Models.DTO.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDtos> OrderItems { get; set; }
    }
}