using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests.Integrations;

[Collection("Database collection")]
public class ManagerControllerIntegrationTests
{
    private readonly PersonManagerContext _db;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;

    public ManagerControllerIntegrationTests(BaseIntegrationTests @base)
    {
        _db = @base.Db;
        _client = @base.Client;
        _scope = @base.Scope;
        @base.ResetDatabase();
    }

    [Fact]
    public async Task GetAll_GetsAllManagers_Correctly()
    {
        var config = _scope.ServiceProvider.GetRequiredService<IOptions<ApiConfiguration>>();
        var results = await _client.GetAsync($"{config.Value.BaseUrl}/api/managers");
        var serialised = await results.Content.ReadFromJsonAsync<
            IEnumerable<ShortManagerViewModel>
        >();

        Assert.Equal(5, serialised?.Count());
    }
}
