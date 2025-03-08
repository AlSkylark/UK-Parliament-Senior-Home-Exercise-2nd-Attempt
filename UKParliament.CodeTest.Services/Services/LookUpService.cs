using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Helpers;
using UKParliament.CodeTest.Services.Mappers.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Services.Services;

public class LookUpService(
    ILookupRepository<Department> departmentRepo,
    ILookupRepository<PayBand> payBandRepo,
    ILookupMapper mapper
) : ILookUpService
{
    public async Task<Department?> GetDepartment(int id)
    {
        return await departmentRepo.GetById(id);
    }

    public async Task<PayBand?> GetPayBand(int id)
    {
        return await payBandRepo.GetById(id);
    }

    public IEnumerable<Department> SearchDepartments(string? name)
    {
        return SearchLookupItem(name, departmentRepo);
    }

    public IEnumerable<PayBand> SearchPayBands(string? name)
    {
        return SearchLookupItem(name, payBandRepo);
    }

    public IEnumerable<string> GetLookupItems()
    {
        return EnumHelper.GetNameValues(typeof(LookupItemsEnum));
    }

    public IEnumerable<LookupItem> LookupItem(LookupItemsEnum? item)
    {
        return item switch
        {
            LookupItemsEnum.Department => SearchLookupItem(null, departmentRepo)
                .Select(mapper.MapToSimple),
            LookupItemsEnum.PayBand => SearchLookupItem(null, payBandRepo)
                .Select(mapper.MapToSimple),
            LookupItemsEnum.EmployeeType => EnumHelper
                .GetNameValues(typeof(EmployeeTypeEnum))
                .Select(mapper.MapFromString)
                .OrderBy(i => i.Name),
            null => [],
            _ => [],
        };
    }

    private static IEnumerable<T> SearchLookupItem<T>(string? name, ILookupRepository<T> repo)
        where T : BaseEntity, ILookupItem
    {
        var items = repo.Search();

        if (name is not null)
        {
            items = items.Where(d =>
                string.Equals(d.Name, name, StringComparison.CurrentCultureIgnoreCase)
            );
        }

        return items.OrderBy(i => i.Name).AsEnumerable();
    }
}
