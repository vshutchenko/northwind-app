using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels.Customer
{
    public class CustomerListViewModel
    {
        public IEnumerable<CustomerViewModel> Customers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
