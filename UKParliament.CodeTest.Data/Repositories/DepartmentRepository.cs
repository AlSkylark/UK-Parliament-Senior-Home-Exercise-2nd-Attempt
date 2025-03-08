using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.Repositories;

public class DepartmentRepository(PersonManagerContext db) : LookupRepository<Department>(db)
{
    public override IQueryable<Department> Search()
    {
        return _db.Departments.AsQueryable();
    }
}
