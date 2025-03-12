using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Tests.Integrations;
using Xunit;

namespace UKParliament.CodeTest.Tests;

[Collection("Database collection")]
public class EmployeeRepositoryTests
{
    private readonly PersonManagerContext _db;

    public EmployeeRepositoryTests(BaseIntegrationTests @base)
    {
        _db = @base.Db;
        @base.ResetDatabase();
    }

    [Fact]
    public async Task GetById_ReturnsEmployee_Successfully()
    {
        var repo = new EmployeeRepository(_db);

        var result = await repo.GetById(1);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
        });
    }

    [Fact]
    public async Task Update_ModifiesEmployee_Successfully()
    {
        var repo = new EmployeeRepository(_db);
        var toModify = await repo.GetById(1);

        toModify!.FirstName = "Test";
        toModify.LastName = "Erino";
        _db.ChangeTracker.Clear();

        var result = await repo.Update(toModify);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Erino", result.LastName);
        });
    }
}
