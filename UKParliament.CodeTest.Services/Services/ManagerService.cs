using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Data.Requests;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Mappers.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Services.Services;

public class ManagerService(
    IManagerRepository managerRepository,
    IManagerMapper mapper,
    IEmployeeMapper employeeMapper
) : IManagerService
{
    public async Task<ManagerViewModel?> Get(int id)
    {
        var result = await managerRepository.GetById(id);
        if (result is null)
        {
            return null;
        }

        var mapped = mapper.Map(result);

        return mapped;
    }

    public IEnumerable<ShortManagerViewModel> GetAll()
    {
        var request = new SearchRequest { EmployeeType = Data.Models.EmployeeTypeEnum.Manager };
        var result = managerRepository.Search(request).Select(employeeMapper.MapManager);

        return result;
    }

    public async Task<ManagerViewModel?> Update(ManagerViewModel model)
    {
        var existing = await managerRepository.GetById(model.Id ?? 0);
        if (existing is null)
        {
            return null;
        }

        var mappedRequest = mapper.MapForSave(model, existing);
        var updated = await managerRepository.Update(mappedRequest);

        var updatedMapped = mapper.Map(updated);
        if (updatedMapped is null)
        {
            return null;
        }

        return updatedMapped;
    }
}
