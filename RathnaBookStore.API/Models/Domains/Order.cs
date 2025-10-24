namespace RathnaBookStore.API.Models.Domains
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        //Navigate Order Items
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
