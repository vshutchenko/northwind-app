using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels.User
{
    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
