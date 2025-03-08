using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Data.Requests;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.Mappers.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Services.Services;

public class EmployeeService(
    IEmployeeRepository repo,
    IEmployeeMapper mapper,
    IPaginatorService paginatorService
) : IEmployeeService
{
    const string PATH = "employee";

    public async Task<EmployeeViewModel?> Create(EmployeeViewModel model)
    {
        var mappedRequest = mapper.MapForCreate(model);

        var result = await repo.Create(mappedRequest);
        var mappedResult = mapper.Map(result);

        return mappedResult;
    }

    public async Task Delete(int id)
    {
        await repo.Delete(id);
    }

    public async Task<IResourceCollection<EmployeeViewModel>> Search(SearchRequest? request)
    {
        var defaultRequest = request ?? new();
        var query = repo.Search(request);

        var data = paginatorService.Paginate(query, defaultRequest);
        var pagination = await paginatorService.CreateAsync(query, defaultRequest, PATH);

        var results = data.Select(mapper.Map);

        var collection = new ResourceCollection<EmployeeViewModel>
        {
            Pagination = pagination,
            Results = results,
        };

        return collection;
    }

    public async Task<EmployeeViewModel?> Update(EmployeeViewModel model)
    {
        var existing = await repo.GetById(model.Id ?? 0);
        if (existing is null || existing.EmployeeType != EmployeeTypeEnum.Employee)
        {
            return null;
        }

        var mappedRequest = mapper.MapForSave(model, existing);
        var updated = await repo.Update(mappedRequest);

        var updatedMapped = mapper.Map(updated);
        if (updatedMapped is null)
        {
            return null;
        }

        return updatedMapped;
    }

    public async Task<EmployeeViewModel?> View(int id)
    {
        var employee = await repo.GetById(id);
        if (employee is null)
        {
            return null;
        }

        var mapped = mapper.Map(employee);
        return mapped;
    }
}
