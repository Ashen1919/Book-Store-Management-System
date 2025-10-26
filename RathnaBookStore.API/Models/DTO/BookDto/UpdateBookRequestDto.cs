using System.ComponentModel.DataAnnotations;

namespace RathnaBookStore.API.Models.DTO.BookDto
{
    public class UpdateBookRequestDto
    {
        [Required]
        public string ISBN { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name should has maximum length of 100 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name should has maximum length of 100 characters")]
        public string Author { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name should has maximum length of 100 characters")]
        public string Category { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ImageUrl { get; set; }

    }
}
