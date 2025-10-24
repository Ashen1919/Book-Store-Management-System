using System.ComponentModel.DataAnnotations;

namespace RathnaBookStore.API.Models.DTO.CategoryDto
{
    public class UpdateCategoryRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name should has maximum length of 100 characters")]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
