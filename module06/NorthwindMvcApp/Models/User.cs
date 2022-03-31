using Microsoft.AspNetCore.Identity;

namespace NorthwindMvcApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public string NorthwindDbId { get; set; }
    }
}
