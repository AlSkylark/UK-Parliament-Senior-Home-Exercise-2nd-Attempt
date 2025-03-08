using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.HATEOAS.Interfaces;

namespace UKParliament.CodeTest.Services.HATEOAS.Interfaces;

public interface IPaginatorService
{
    /// <summary>
    /// Creates a new Pagination object.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="request"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public Task<Pagination> CreateAsync(
        IQueryable<object> query,
        IPaginatable request,
        string path = ""
    );

    /// <summary>
    /// Paginates an IQueryable given the requests' parameters.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public IQueryable<T> Paginate<T>(IQueryable<T> query, IPaginatable request);
}
