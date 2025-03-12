using FluentValidation;
using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.Mappers.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Services.Mappers;

public class ManagerMapper(
    IValidator<Employee> irregularityValidator,
    IEmployeeMapper mapper,
    IResourceService<EmployeeViewModel> resourceService,
    ILookUpService lookUpService
)
    : BasePersonMapper<ManagerViewModel, Manager>(irregularityValidator, lookUpService),
        IManagerMapper
{
    public override ManagerViewModel Map(Manager manager)
    {
        var mapped = base.Map(manager);
        mapped.Employees = manager
            .Employees.Select(mapper.Map)
            .Select(e =>
                (Resource<EmployeeViewModel>)resourceService.GenerateResource(e, "employee")
            );

        return mapped;
    }
}
