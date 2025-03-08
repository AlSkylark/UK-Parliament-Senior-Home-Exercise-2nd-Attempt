using Microsoft.Extensions.Options;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.Helpers;

namespace UKParliament.CodeTest.Services.HATEOAS;

public abstract class BaseResourceService<T>(IOptions<ApiConfiguration> config)
    : IResourceService<T>
    where T : BaseViewModel
{
    protected readonly ApiConfiguration _config = config.Value;

    public virtual IResource<IResourceCollection<IResource<T>>> GenerateCollectionResource(
        IResourceCollection<IResource<T>> collection,
        string path
    )
    {
        return new Resource<IResourceCollection<IResource<T>>>
        {
            Data = collection,
            Links =
            [
                Link.GenerateLink(
                    "self",
                    UrlHelpers.Generate(_config.BaseUrl, _config.ApiPrefix, path),
                    "GET",
                    "POST"
                ),
            ],
        };
    }

    public virtual IResource<T> GenerateResource(T data, string path)
    {
        return new Resource<T>
        {
            Data = data,
            Links =
            [
                Link.GenerateLink(
                    "self",
                    UrlHelpers.Generate(_config.BaseUrl, _config.ApiPrefix, path, $"{data.Id}"),
                    "GET",
                    "PUT",
                    "DELETE"
                ),
            ],
        };
    }
}
