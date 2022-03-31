using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels.Product
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
