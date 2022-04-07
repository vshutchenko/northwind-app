using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels.Article
{
    public class BlogArticleListViewModel
    {
        public IEnumerable<BlogArticleViewModel> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
