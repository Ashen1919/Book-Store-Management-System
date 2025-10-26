using RathnaBookStore.API.Models.DTO.OrderItemDto;

namespace RathnaBookStore.API.Models.DTO.OrderDto
{
    public class CreateOrderDto
    {
        public decimal Discount { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}
