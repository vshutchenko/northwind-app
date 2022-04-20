using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels.Employee
{
    public class EmployeeListViewModel
    {
        public IEnumerable<EmployeeViewModel> Employees { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
