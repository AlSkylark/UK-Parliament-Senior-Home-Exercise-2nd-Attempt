using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.Repositories.Interfaces
{
    public interface IManagerRepository : IBasePersonRepository<Manager>
    {
        IQueryable<Employee> GetManagerEmployees(int managerId);
    }
}
