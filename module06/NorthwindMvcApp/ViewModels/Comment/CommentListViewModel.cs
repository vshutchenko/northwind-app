using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels.Comment
{
    public class CommentListViewModel
    {
        public IEnumerable<BlogCommentViewModel> Comments { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
