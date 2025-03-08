namespace UKParliament.CodeTest.Services.Helpers;

public static class UrlHelpers
{
    public static string Generate(string baseUrl, params string[] additions)
    {
        return Generate(baseUrl, null, additions);
    }

    public static string Generate(
        string baseUrl,
        IEnumerable<KeyValuePair<string, object?>>? parameters
    )
    {
        return Generate(baseUrl, parameters, null);
    }

    public static string Generate(
        string baseUrl,
        IEnumerable<KeyValuePair<string, object?>>? parameters = null,
        params string[]? additions
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        var uri = new UriBuilder(baseUrl);
        var rawAdditions = additions
            ?.Aggregate("", (prev, current) => prev += $"{current.Trim('/')}/")
            .TrimEnd('/');
        var query = parameters
            ?.Aggregate(
                "?",
                (prev, current) =>
                {
                    if (current.Value is not null)
                        prev += $"{current.Key}={current.Value}&";
                    return prev;
                }
            )
            .TrimEnd('&');

        if (!string.IsNullOrEmpty(rawAdditions))
        {
            uri.Path = rawAdditions;
        }
        uri.Query = query;

        return uri.Uri.AbsoluteUri;
    }
}
