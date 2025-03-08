using FluentAssertions;
using UKParliament.CodeTest.Services.Helpers;
using Xunit;

namespace UKParliament.CodeTest.Tests;

public class UrlHelpersTests
{
    [Fact]
    public void Generate_WhenBaseIsNull_Throws()
    {
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentNullException>(() => UrlHelpers.Generate(null!));
            Assert.Throws<ArgumentException>(() => UrlHelpers.Generate(""));
            Assert.Throws<ArgumentException>(() => UrlHelpers.Generate("  "));
        });
    }

    [Fact]
    public void Generate_FormatsWeirdUrl_Correctly()
    {
        var nothing = UrlHelpers.Generate("test.com");
        var httpNoTrail = UrlHelpers.Generate("https://test");

        Assert.Multiple(() =>
        {
            nothing.Should().Be("http://test.com/");
            httpNoTrail.Should().Be("https://test/");
        });
    }

    [Fact]
    public void Generate_FormatsStandardUrl_Correctly()
    {
        var baseUrl = "https://test.com/";
        var result = UrlHelpers.Generate(baseUrl);

        result.Should().Be($"{baseUrl}");
    }

    [Fact]
    public void Generate_WhenNoSlashInBase_ShouldAddOne()
    {
        var baseUrl = "https://test.com";
        var result = UrlHelpers.Generate(baseUrl);

        result.Should().Be($"{baseUrl}/");
    }

    [Fact]
    public void Generate_FormatsWithQuery_Correctly()
    {
        var baseUrl = "https://test.com/";
        var query = GenerateQueries(1);
        var result = UrlHelpers.Generate(baseUrl, query, null);

        result
            .Should()
            .Be($"{baseUrl}{query.Aggregate("?", QueryAggregator)
            .TrimEnd('&')}");
    }

    [Theory]
    [InlineData('/', true)]
    [InlineData('/', false)]
    [InlineData(null, true)]
    [InlineData(null, false)]
    public void Generate_FormatsWithPath_Correctly(char? slash, bool twoPaths)
    {
        var baseUrl = "https://test.com";
        IEnumerable<string> path = ["user", $"{slash}form"];
        var result = UrlHelpers.Generate(baseUrl, path.Take(twoPaths ? 2 : 1).ToArray());

        var expected = $"{baseUrl}/" + (twoPaths ? "user/form" : "user");

        result.Should().Be($"{expected}");
    }

    [Fact]
    public void Generate_FormatsWithPathAndQuery_Correctly()
    {
        var baseUrl = "https://test.com";
        IEnumerable<string> path = ["user", $"form"];
        var query = GenerateQueries(1);
        var result = UrlHelpers.Generate(baseUrl, query, path.ToArray());

        var expected =
            $"{baseUrl}/" + "user/form" + query.Aggregate("?", QueryAggregator).TrimEnd('&');

        result.Should().Be($"{expected}");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(5)]
    public void Generate_FormatsWithMultiples_Correctly(int number)
    {
        var baseUrl = "https://test.com";
        IEnumerable<string> path = ["user", $"form"];
        var query = GenerateQueries(number);
        var result = UrlHelpers.Generate(baseUrl, query, path.ToArray());

        var expected =
            $"{baseUrl}/" + "user/form" + query.Aggregate("?", QueryAggregator).TrimEnd('&');

        result.Should().Be($"{expected}");
    }

    private static readonly Func<string, KeyValuePair<string, object?>, string> QueryAggregator = (
        prev,
        current
    ) =>
    {
        if (current.Value is not null)
            prev += $"{current.Key}={current.Value}&";
        return prev;
    };

    private static IEnumerable<KeyValuePair<string, object?>> GenerateQueries(int number)
    {
        var dict = new Dictionary<string, object?>
        {
            { "page", 1 },
            { "skip", 10 },
            { "take", 20 },
            { "textSearch", "egh" },
            { "empty", null },
        };

        return dict.Take(number);
    }
}
