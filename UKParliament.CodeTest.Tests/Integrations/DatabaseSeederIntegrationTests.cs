using UKParliament.CodeTest.Data;
using Xunit;

namespace UKParliament.CodeTest.Tests.Integrations;

[Collection("Database collection")]
public class DatabaseSeederIntegrationTests
{
    private readonly PersonManagerContext _db;

    public DatabaseSeederIntegrationTests(BaseIntegrationTests @base)
    {
        _db = @base.Db;
        @base.ResetDatabase();
    }

    [Fact]
    public void SeedDepartments_Seeds_Successfully()
    {
        var results = _db.Departments.ToList();

        Assert.Equal(4, results.Count);
    }

    [Fact]
    public void SeedPayBands_Seeds_Successfully()
    {
        var results = _db.PayBands.ToList();

        Assert.Equal(6, results.Count);
    }

    [Fact]
    public void CreateFakeManager_Seeds_Successfully()
    {
        var results = _db.Managers.ToList();
        var employeeCount = results.Select(m => m.Employees.Count()).Aggregate(0, (s, n) => s += n);

        Assert.Equal(5, results.Count);
        Assert.Equal(50, employeeCount);
    }
}
