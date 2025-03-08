using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.Helpers;

namespace UKParliament.CodeTest.Services.HATEOAS;

public class PaginatorService(IOptions<ApiConfiguration> config) : IPaginatorService
{
    private readonly ApiConfiguration _config = config.Value;
    private readonly Dictionary<string, object?> _queries = [];

    public async Task<Pagination> CreateAsync(
        IQueryable<object> query,
        IPaginatable request,
        string path = ""
    )
    {
        var count = await query.CountAsync();
        var limit = request.Limit;
        var page = request.Page <= 0 ? 1 : request.Page;

        if (limit > 0)
            _queries.Add("limit", limit);

        var baseUrl = UrlHelpers.Generate(_config.BaseUrl, [_config.ApiPrefix, path]);
        var final = limit > 0 ? (int)Math.Ceiling((double)count / limit) : 1;

        return new()
        {
            Total = count,
            PerPage = limit == 0 ? _config.DefaultLimit : limit,
            CurrentPage = page,
            FinalPage = final,
            FirstPageUrl = GeneratePage(1),
            FinalPageUrl = GeneratePage(final),
            NextPageUrl = GeneratePage(page >= final ? null : page + 1),
            PrevPageUrl = GeneratePage(page > 1 && page <= final ? page - 1 : null),
            Path = baseUrl,
            From = CalculateFrom(),
            To = CalculateTo(),
        };

        int CalculateFrom()
        {
            if (page == 1)
                return 1;
            if (page > final)
                return 0;

            return (page - 1) * limit + 1;
        }

        int CalculateTo()
        {
            if (page == final)
                return count;
            if (page > final)
                return 0;

            return page * limit;
        }

        string? GeneratePage(int? page)
        {
            if (_queries.ContainsKey("page"))
            {
                _queries["page"] = page;
            }
            else
            {
                _queries.Add("page", page);
            }
            var url = UrlHelpers.Generate(baseUrl, _queries);

            return page is null ? null : url;
        }
    }

    //TODO: Unit test this!
    public IQueryable<T> Paginate<T>(IQueryable<T> query, IPaginatable request)
    {
        var limit = request.Limit == 0 ? _config.DefaultLimit : request.Limit;
        var page = request.Page <= 0 ? 1 : request.Page;

        return query.Skip((page - 1) * limit).Take(limit);
    }
}
