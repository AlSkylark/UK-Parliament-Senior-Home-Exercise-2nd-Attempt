using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.Repositories.Interfaces;

public interface IEmployeeRepository : IBasePersonRepository<Employee>
{
    IQueryable<Employee> GetManagerEmployees(int managerId);
}
