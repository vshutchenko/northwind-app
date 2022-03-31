using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels.Category
{
    public class CategoryListViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
