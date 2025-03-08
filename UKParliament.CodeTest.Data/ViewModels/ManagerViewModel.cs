namespace UKParliament.CodeTest.Data.ViewModels;

public class ManagerViewModel : EmployeeViewModel
{
    public IEnumerable<EmployeeViewModel> Employees { get; set; } = [];
}
