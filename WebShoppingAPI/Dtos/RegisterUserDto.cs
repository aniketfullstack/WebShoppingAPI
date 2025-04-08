using System.ComponentModel.DataAnnotations;

namespace WebShoppingAPI.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public required string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 4)]
        public required string Password { get; set; }
    }
}
