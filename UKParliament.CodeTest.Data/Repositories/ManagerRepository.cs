using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Repositories.Interfaces;

namespace UKParliament.CodeTest.Data.Repositories
{
    public class ManagerRepository(PersonManagerContext db)
        : BasePersonRepository<Manager>(db),
            IManagerRepository
    {
        protected override IQueryable<Manager> PrepareGetQuery(DbSet<Manager> set)
        {
            return base.PrepareGetQuery(set)
                .Include(p => p.Address)
                .Include(p => p.PayBand)
                .Include(p => p.Department)
                .Include(e => e.Employees)
                .ThenInclude(e => e.PayBand)
                .Include(e => e.Employees)
                .ThenInclude(e => e.Department);
        }

        public IQueryable<Employee> GetManagerEmployees(int managerId)
        {
            var query = base.Search().Where(e => e.ManagerId == managerId);

            return query;
        }
    }
}
