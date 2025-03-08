using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Repositories.Interfaces;

namespace UKParliament.CodeTest.Data.Repositories;

public abstract class BaseRepository<T>(PersonManagerContext db) : IBaseRepository<T>
    where T : BaseEntity
{
    protected readonly PersonManagerContext _db = db;

    public virtual async Task<T> Create(T entity)
    {
        _db.Add(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public virtual async Task Delete(int id)
    {
        var entity =
            await _db.Set<T>().Where(p => p.Id == id).FirstOrDefaultAsync()
            ?? throw new Exception();
        _db.Remove(entity);

        await _db.SaveChangesAsync();
    }

    public abstract IQueryable<T> Search();

    public virtual async Task<T> Update(T entity)
    {
        _db.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T?> GetById(int id)
    {
        return await _db.Set<T>()
            .Where(p => p.Id == id)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync();
    }
}
