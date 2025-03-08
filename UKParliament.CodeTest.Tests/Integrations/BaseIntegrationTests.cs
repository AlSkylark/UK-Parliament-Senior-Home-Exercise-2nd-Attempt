using Microsoft.Extensions.DependencyInjection;
using UKParliament.CodeTest.Data;
using Xunit;

namespace UKParliament.CodeTest.Tests.Integrations;

public class BaseIntegrationTests : IDisposable
{
    public IntegrationTestsWebAppFactory Factory;
    public IServiceScope Scope;
    public PersonManagerContext Db;
    public HttpClient Client;

    public BaseIntegrationTests()
    {
        Factory = new IntegrationTestsWebAppFactory();

        Scope = Factory.Services.CreateScope();
        Db = Scope.ServiceProvider.GetRequiredService<PersonManagerContext>();
        Client = Factory.CreateClient();
    }

    public void ResetDatabase()
    {
        Db.ChangeTracker.Clear();
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        Db.SeedDatabase();
    }

    public void Dispose()
    {
        Db.Dispose();
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<BaseIntegrationTests> { }
