using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.Repositories.Interfaces;

public interface IBaseRepository<T>
    where T : BaseEntity
{
    Task<T> Create(T entity);
    Task<T?> GetById(int id);
    IQueryable<T> Search();
    Task<T> Update(T entity);
    Task Delete(int id);
}
