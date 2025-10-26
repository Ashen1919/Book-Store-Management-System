using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Models.DTO.OrderItemDto
{
    public class OrderItemDtos
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
