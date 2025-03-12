using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests.Integrations;

[Collection("Database collection")]
public class EmployeeControllerIntegrationTests
{
    private readonly PersonManagerContext _db;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly ApiConfiguration _config;

    public EmployeeControllerIntegrationTests(BaseIntegrationTests @base)
    {
        _db = @base.Db;
        _client = @base.Client;
        _scope = @base.Scope;
        _config = _scope.ServiceProvider.GetRequiredService<IOptions<ApiConfiguration>>().Value;
        @base.ResetDatabase();
    }

    [Fact]
    public async Task View_ReturnsEmployee_Correctly()
    {
        var expectedEmployee = new Employee { FirstName = "Alex", LastName = "Castro" };
        await _db.Employees.AddAsync(expectedEmployee);
        await _db.SaveChangesAsync();

        var result = await _client.GetAsync(
            $"{_config.BaseUrl}/api/employee/{expectedEmployee.Id}"
        );
        var deserialised = await result.Content.ReadFromJsonAsync<Resource<EmployeeViewModel>>();

        Assert.Multiple(() =>
        {
            Assert.NotNull(deserialised);
            Assert.Equal("Alex", deserialised.Data.FirstName);
            Assert.Equal("Castro", deserialised.Data.LastName);
        });
    }

    [Fact]
    public async Task Post_CreatesEmployee_Correctly()
    {
        var expectedEmployee = new EmployeeViewModel { FirstName = "Alex", LastName = "Castro" };

        var result = await _client.PostAsJsonAsync(
            $"{_config.BaseUrl}/api/employee",
            expectedEmployee
        );
        var deserialised = await result.Content.ReadFromJsonAsync<Resource<EmployeeViewModel>>();

        Assert.Multiple(() =>
        {
            Assert.NotNull(deserialised);
            Assert.Equal("Alex", deserialised.Data.FirstName);
            Assert.Equal("Castro", deserialised.Data.LastName);
        });
    }

    [Fact]
    public async Task Put_UpdatesEmployee_Correctly()
    {
        var original = new Employee { FirstName = "Alex", LastName = "Castro" };
        await _db.Employees.AddAsync(original);
        await _db.SaveChangesAsync();
        var updated = new EmployeeViewModel
        {
            Id = original.Id,
            FirstName = "Test",
            LastName = "Update",
        };

        var result = await _client.PutAsJsonAsync(
            $"{_config.BaseUrl}/api/employee/{original.Id}",
            updated
        );
        var deserialised = await result.Content.ReadFromJsonAsync<Resource<EmployeeViewModel>>();

        Assert.Multiple(() =>
        {
            Assert.NotNull(deserialised);
            Assert.Equal("Test", deserialised.Data.FirstName);
            Assert.Equal("Update", deserialised.Data.LastName);
        });
    }

    [Fact]
    public async Task Delete_DeletesEmployee_Correctly()
    {
        var original = new Employee { FirstName = "Alex", LastName = "Castro" };
        await _db.Employees.AddAsync(original);
        await _db.SaveChangesAsync();
        var url = $"{_config.BaseUrl}/api/employee/{original.Id}";

        var result = await _client.DeleteAsync(url);
        Assert.True(result.StatusCode == System.Net.HttpStatusCode.NoContent);

        var attempt = await _client.GetAsync(url);
        Assert.True(attempt.StatusCode == System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Search_FindsEmployees_Correctly()
    {
        var expectedEmployee = new Employee
        {
            FirstName = "Alex",
            LastName = "Castro",
            DepartmentId = 3, //finance
        };
        await _db.Employees.AddAsync(expectedEmployee);
        await _db.SaveChangesAsync();

        var result = await _client.GetAsync(
            $"{_config.BaseUrl}/api/employee?department=Finance&textSearch=Castr"
        );
        var deserialised = await result.Content.ReadFromJsonAsync<
            Resource<ResourceCollection<Resource<EmployeeViewModel>>>
        >();

        Assert.Multiple(() =>
        {
            Assert.NotNull(deserialised);
            Assert.Single(deserialised.Data.Results);
            var toTest = deserialised.Data.Results.First();
            Assert.Equal("Alex", toTest.Data.FirstName);
            Assert.Equal("Castro", toTest.Data.LastName);
            Assert.Equal("Finance", toTest.Data.Department);
        });
    }

    [Fact]
    public async Task Search_WithoutQueryParams_FindsAllEmployees()
    {
        var result = await _client.GetAsync($"{_config.BaseUrl}/api/employee");
        var deserialised = await result.Content.ReadFromJsonAsync<
            Resource<ResourceCollection<Resource<EmployeeViewModel>>>
        >();

        Assert.Multiple(() =>
        {
            Assert.NotNull(deserialised);
            Assert.Equal(55, deserialised.Data.Pagination.Total);
        });
    }

    [Fact]
    public async Task Search_WhenFilteringManagers_FindsAllManagers()
    {
        var result = await _client.GetAsync($"{_config.BaseUrl}/api/employee?employeeType=manager");
        var deserialised = await result.Content.ReadFromJsonAsync<
            Resource<ResourceCollection<Resource<EmployeeViewModel>>>
        >();

        Assert.Multiple(() =>
        {
            Assert.NotNull(deserialised);
            Assert.Equal(5, deserialised.Data.Results.Count());
        });
    }
}
