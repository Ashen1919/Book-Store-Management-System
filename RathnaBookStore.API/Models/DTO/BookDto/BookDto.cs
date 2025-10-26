namespace RathnaBookStore.API.Models.DTO.BookDto
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
