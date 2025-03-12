using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Data.Requests;

namespace UKParliament.CodeTest.Data.Repositories;

public class EmployeeRepository(PersonManagerContext db)
    : BasePersonRepository<Employee>(db),
        IEmployeeRepository
{
    protected override IQueryable<Employee> CreateSearchQuery(SearchRequest? request)
    {
        var query = base.CreateSearchQuery(request);

        if (request is not null)
        {
            query = SearchByPayBand(query, request);
            query = SearchByDepartment(query, request);
        }

        return query.Include(p => p.PayBand).Include(p => p.Department).Include(p => p.Manager);
    }

    protected override IQueryable<Employee> PrepareGetQuery(DbSet<Employee> set)
    {
        var query = base.PrepareGetQuery(set)
            .Include(p => p.Address)
            .Include(p => p.PayBand)
            .Include(p => p.Department)
            .Include(p => p.Manager);

        return query;
    }
}
