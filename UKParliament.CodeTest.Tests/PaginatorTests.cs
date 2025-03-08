using FakeItEasy;
using FluentAssertions;
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
            result.Should().NotBeNull();
            result.CurrentPage.Should().Be(page);
            result.FinalPage.Should().Be(5);
            result.PerPage.Should().Be(limit);
            result.Total.Should().Be(50);
            result.From.Should().Be(21);
            result.To.Should().Be(30);
            result.Path.Should().Be(expectedUrl);
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
            result.FirstPageUrl.Should().Be($"{expectedUrl}?limit=1&page=1");
            result.FinalPageUrl.Should().Be($"{expectedUrl}?limit=1&page=5");
            result.NextPageUrl.Should().Be($"{expectedUrl}?limit=1&page=4");
            result.PrevPageUrl.Should().Be($"{expectedUrl}?limit=1&page=2");
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
            result.CurrentPage.Should().Be(66);
            result.From.Should().Be(0);
            result.To.Should().Be(0);
            result.FirstPageUrl.Should().Be($"{expectedUrl}?limit=1&page=1");
            result.FinalPageUrl.Should().Be($"{expectedUrl}?limit=1&page=5");
            result.NextPageUrl.Should().Be(null);
            result.PrevPageUrl.Should().Be(null);
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
            result.PerPage.Should().Be(_options.Value.DefaultLimit);
            result.Total.Should().Be(50);
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
            result.CurrentPage.Should().Be(1);
            result.Total.Should().Be(50);
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
            result.Should().HaveCount(10);
            result[0].Should().Be(firstResult);
            result[9].Should().Be(secondResult);
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

        result.Should().HaveCount(0);
    }
}
