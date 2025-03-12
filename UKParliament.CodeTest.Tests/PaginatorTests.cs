using FakeItEasy;
using Microsoft.Extensions.Options;
using MockQueryable;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Requests;
using UKParliament.CodeTest.Services.HATEOAS;
using Xunit;

namespace UKParliament.CodeTest.Tests;

public class PaginatorTests
{
    private IOptions<ApiConfiguration> _options;
    private PaginatorService _paginator;

    public PaginatorTests()
    {
        _options = A.Fake<IOptions<ApiConfiguration>>();
        _options.Value.BaseUrl = "https://test.com/";
        _options.Value.ApiPrefix = "api";
        _options.Value.DefaultLimit = 20;

        _paginator = new PaginatorService(_options);
    }

    [Fact]
    public async Task Create_GeneratesBasicDetails_Correctly()
    {
        const int limit = 10;
        const int page = 3;
        IQueryable<string> list = A.CollectionOfDummy<string>(50).BuildMock();
        var request = new SearchRequest { Page = page, Limit = limit };

        var result = await _paginator.CreateAsync(list, request);

        const string expectedUrl = "https://test.com/api";
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(page, result.CurrentPage);
            Assert.Equal(5, result.FinalPage);
            Assert.Equal(limit, result.PerPage);
            Assert.Equal(50, result.Total);
            Assert.Equal(21, result.From);
            Assert.Equal(30, result.To);
            Assert.Equal(expectedUrl, result.Path);
        });
    }

    [Fact]
    public async Task Create_GeneratesUrls_Correctly()
    {
        const int limit = 1;
        const int page = 3;
        IQueryable<string> list = A.CollectionOfDummy<string>(5).BuildMock();
        var request = new SearchRequest { Page = page, Limit = limit };

        var result = await _paginator.CreateAsync(list, request);

        const string expectedUrl = "https://test.com/api";
        Assert.Multiple(() =>
        {
            Assert.Equal($"{expectedUrl}?limit=1&page=1", result.FirstPageUrl);
            Assert.Equal($"{expectedUrl}?limit=1&page=5", result.FinalPageUrl);
            Assert.Equal($"{expectedUrl}?limit=1&page=4", result.NextPageUrl);
            Assert.Equal($"{expectedUrl}?limit=1&page=2", result.PrevPageUrl);
        });
    }

    [Fact]
    public async Task Create_WhenPageIsHigherThanCount_GeneratesFinalPage()
    {
        const int limit = 1;
        const int page = 66;
        IQueryable<string> list = A.CollectionOfDummy<string>(5).BuildMock();
        var request = new SearchRequest { Page = page, Limit = limit };

        var result = await _paginator.CreateAsync(list, request);

        const string expectedUrl = "https://test.com/api";
        Assert.Multiple(() =>
        {
            Assert.Equal(66, result.CurrentPage);
            Assert.Equal(0, result.From);
            Assert.Equal(0, result.To);
            Assert.Equal($"{expectedUrl}?limit=1&page=1", result.FirstPageUrl);
            Assert.Equal($"{expectedUrl}?limit=1&page=5", result.FinalPageUrl);
            Assert.Null(result.NextPageUrl);
            Assert.Null(result.PrevPageUrl);
        });
    }

    [Fact]
    public async Task Create_WhenLimitIsZero_DefaultIsUsed()
    {
        const int limit = 0;
        const int page = 1;
        IQueryable<string> list = A.CollectionOfDummy<string>(50).BuildMock();
        var request = new SearchRequest { Page = page, Limit = limit };

        var result = await _paginator.CreateAsync(list, request);

        Assert.Multiple(() =>
        {
            Assert.Equal(_options.Value.DefaultLimit, result.PerPage);
            Assert.Equal(50, result.Total);
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99)]
    public async Task Create_WhenPageIsZeroOrNegative_DefaultIsUsed(int page)
    {
        const int limit = 10;
        IQueryable<string> list = A.CollectionOfDummy<string>(50).BuildMock();
        var request = new SearchRequest { Page = page, Limit = limit };

        var result = await _paginator.CreateAsync(list, request);

        Assert.Multiple(() =>
        {
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(50, result.Total);
        });
    }

    [Theory]
    [InlineData(1, 1, 10)]
    [InlineData(2, 11, 20)]
    [InlineData(5, 41, 50)]
    public void Paginate_ReturnsRange_Correctly(int reqPage, int firstResult, int secondResult)
    {
        const int limit = 10;
        IQueryable<int> list = Enumerable.Range(1, 50).AsQueryable();
        var request = new SearchRequest { Page = reqPage, Limit = limit };

        var result = _paginator.Paginate(list, request).ToList();

        Assert.Multiple(() =>
        {
            Assert.Equal(10, result.Count);
            Assert.Equal(firstResult, result[0]);
            Assert.Equal(secondResult, result[9]);
        });
    }

    [Fact]
    public void Paginate_WhenPageIsHigherThanCount_ReturnsEmpty()
    {
        const int limit = 10;
        const int page = 99;
        IQueryable<int> list = Enumerable.Range(1, 50).AsQueryable();
        var request = new SearchRequest { Page = page, Limit = limit };

        var result = _paginator.Paginate(list, request).ToList();

        Assert.Empty(result);
    }
}
