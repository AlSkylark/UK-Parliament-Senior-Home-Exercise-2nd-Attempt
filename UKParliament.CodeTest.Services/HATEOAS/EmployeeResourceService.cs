using Microsoft.Extensions.Options;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Helpers;

namespace UKParliament.CodeTest.Services.HATEOAS;

public class EmployeeResourceService(IOptions<ApiConfiguration> config)
    : BaseResourceService<EmployeeViewModel>(config)
{
    public override IResource<EmployeeViewModel> GenerateResource(
        EmployeeViewModel data,
        string path
    )
    {
        List<Link> links =
        [
            Link.GenerateLink(
                "self",
                UrlHelpers.Generate(_config.BaseUrl, _config.ApiPrefix, path, $"{data.Id}"),
                "GET",
                "PUT",
                "DELETE"
            ),
        ];

        if (data.HasManager)
        {
            links.Add(
                Link.GenerateLink(
                    "manager",
                    UrlHelpers.Generate(
                        _config.BaseUrl,
                        _config.ApiPrefix,
                        path,
                        $"{data.ManagerId}"
                    ),
                    "GET"
                )
            );
        }

        return new Resource<EmployeeViewModel> { Data = data, Links = links };
    }
}
