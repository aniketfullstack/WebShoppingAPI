using System.ComponentModel.DataAnnotations;

namespace WebShoppingAPI.Dtos
{
    public class RolesDto
    {

        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<User>? Users { get; set; } = [];
    }

    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
