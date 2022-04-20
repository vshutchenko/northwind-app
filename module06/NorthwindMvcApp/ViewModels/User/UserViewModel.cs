namespace NorthwindMvcApp.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string NorthwindDbId { get; set; }
    }
}
