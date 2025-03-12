using Microsoft.Extensions.Options;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.HATEOAS
{
    class ManagerResourceService(IOptions<ApiConfiguration> config)
        : BaseResourceService<ManagerViewModel>(config) { }
}
