using UKParliament.CodeTest.Data.HATEOAS;

namespace UKParliament.CodeTest.Data.ViewModels;

public class ManagerViewModel : EmployeeViewModel
{
    public IEnumerable<Resource<EmployeeViewModel>> Employees { get; set; } = [];
}
