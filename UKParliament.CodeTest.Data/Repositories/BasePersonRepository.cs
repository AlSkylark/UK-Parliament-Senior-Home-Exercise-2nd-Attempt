using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Requests;

namespace UKParliament.CodeTest.Data.Repositories;

public abstract class BasePersonRepository<T>(PersonManagerContext db) : BaseRepository<T>(db)
    where T : Employee
{
    /// <summary>
    /// Allows modification of the main Search method, to, for example,
    /// add type specific includes. E.g: <c>Manager.Employees</c>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected virtual IQueryable<T> CreateSearchQuery(SearchRequest? request)
    {
        var query = _db.Set<T>().AsQueryable();
        if (request is not null)
        {
            query = SearchByText(request, query);
            query = SearchByType(request, query);
        }

        return query.Include(p => p.Address).OrderBy(p => p.LastName);
    }

    public override IQueryable<T> Search()
    {
        return Search(null);
    }

    public IQueryable<T> Search(SearchRequest? request)
    {
        var query = CreateSearchQuery(request);

        return query.OfType<T>();
    }

    public override Task<T?> GetById(int id)
    {
        var set = _db.Set<T>();
        var query = PrepareGetQuery(set);
        return query
            .Where(p => p.Id == id)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync();
    }

    protected virtual IQueryable<T> PrepareGetQuery(DbSet<T> set)
    {
        var query = set.Include(p => p.Address);
        return query;
    }

    protected static IQueryable<T> SearchByType(SearchRequest request, IQueryable<T> query)
    {
        if (request.EmployeeType.HasValue)
        {
            query = query.Where(p => p.EmployeeType == request.EmployeeType);
        }

        return query;
    }

    protected static IQueryable<T> SearchByText(SearchRequest request, IQueryable<T> query)
    {
        if (!string.IsNullOrWhiteSpace(request.TextSearch))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, $"%{request.TextSearch}%")
                || EF.Functions.Like(p.LastName, $"%{request.TextSearch}%")
            );
        }

        return query;
    }

    protected static IQueryable<T> SearchByPayBand(IQueryable<T> query, SearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.PayBand))
        {
            query = query.Where(p =>
                string.Compare((p as Employee)!.PayBand!.Name, request.PayBand, true) == 0
            );
        }
        return query;
    }

    protected static IQueryable<T> SearchByDepartment(IQueryable<T> query, SearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Department))
        {
            query = query.Where(p =>
                string.Compare((p as Employee)!.Department!.Name, request.Department, true) == 0
            );
        }
        return query;
    }
}
