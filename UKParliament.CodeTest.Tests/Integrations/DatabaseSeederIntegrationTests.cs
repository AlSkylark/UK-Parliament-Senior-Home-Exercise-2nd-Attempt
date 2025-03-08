using FluentAssertions;
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

        results.Should().HaveCount(4);
    }

    [Fact]
    public void SeedPayBands_Seeds_Successfully()
    {
        var results = _db.PayBands.ToList();

        results.Should().HaveCount(6);
    }

    [Fact]
    public void CreateFakeManager_Seeds_Successfully()
    {
        var results = _db.Managers.ToList();
        var employeeCount = results.Select(m => m.Employees.Count()).Aggregate(0, (s, n) => s += n);

        results.Should().HaveCount(5);
        employeeCount.Should().Be(50);
    }
}
