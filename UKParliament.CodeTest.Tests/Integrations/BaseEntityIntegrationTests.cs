using FluentAssertions;
using FluentAssertions.Extensions;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Models;
using Xunit;

namespace UKParliament.CodeTest.Tests.Integrations;

[Collection("Database collection")]
public class BaseEntityIntegrationTests
{
    private readonly PersonManagerContext _db;

    public BaseEntityIntegrationTests(BaseIntegrationTests @base)
    {
        _db = @base.Db;
        @base.ResetDatabase();
    }

    [Fact]
    public async Task CreatedUpdatedInterceptor_AddsCreatedAt_Correctly()
    {
        var firstStamp = DateTime.UtcNow;
        const string name = "Test";
        var department = new Department { Name = name };

        _db.Departments.Add(department);
        await _db.SaveChangesAsync();

        var result = _db.Departments.Where(d => d.Name == name).First();

        result.CreatedAt.Should().BeWithin(100.Milliseconds()).After(firstStamp);
    }

    [Fact]
    public async Task CreatedUpdatedInterceptor_UpdatesUpdatedAt_Correctly()
    {
        const string name = "Test";
        var department = new Department { Name = name };

        _db.Departments.Add(department);
        await _db.SaveChangesAsync();

        var result = _db.Departments.Where(d => d.Name == name).First();
        var updatedDate = result.UpdatedAt;

        result.Name = "UpdatedName";

        await Task.Delay(100);
        await _db.SaveChangesAsync();

        Assert.Multiple(() =>
        {
            result.UpdatedAt.Should().NotBe(updatedDate);
            result.UpdatedAt.Should().BeAfter(updatedDate);
        });
    }
}
