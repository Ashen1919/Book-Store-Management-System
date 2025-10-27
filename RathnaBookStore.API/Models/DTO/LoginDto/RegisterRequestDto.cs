using System.ComponentModel.DataAnnotations;

namespace RathnaBookStore.API.Models.DTO.LoginDto
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
