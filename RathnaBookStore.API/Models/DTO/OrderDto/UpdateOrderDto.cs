using RathnaBookStore.API.Models.DTO.OrderItemDto;

namespace RathnaBookStore.API.Models.DTO.OrderDto
{
    public class UpdateOrderDto
    {
        public DateTime OrderTime { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}
