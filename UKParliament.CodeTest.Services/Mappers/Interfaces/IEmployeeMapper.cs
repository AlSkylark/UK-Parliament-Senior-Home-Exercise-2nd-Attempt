using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.Mappers.Interfaces;

public interface IEmployeeMapper
{
    EmployeeViewModel Map(Employee person);
    Employee MapForSave(EmployeeViewModel vm, Employee existing);
    Employee MapForCreate(EmployeeViewModel vm);
    ShortManagerViewModel MapManager(Employee manager);
}
