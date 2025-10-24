namespace RathnaBookStore.API.Models.Domains
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        //Foreign Keys
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        //Item Details
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
