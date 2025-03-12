using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.Mappers.Interfaces;

public interface IEmployeeMapper : IBasePersonMapper<EmployeeViewModel, Employee>
{
    Employee MapForCreate(EmployeeViewModel vm);
}
