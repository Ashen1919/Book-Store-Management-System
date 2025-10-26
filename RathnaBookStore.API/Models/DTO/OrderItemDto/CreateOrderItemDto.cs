namespace RathnaBookStore.API.Models.DTO.OrderItemDto
{
    public class CreateOrderItemDto
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
    }
}
