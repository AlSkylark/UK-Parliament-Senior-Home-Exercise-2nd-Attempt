using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.Services.Interfaces;

public interface ILookUpService
{
    Task<Department?> GetDepartment(int id);
    Task<PayBand?> GetPayBand(int id);

    IEnumerable<Department> SearchDepartments(string? name);
    IEnumerable<PayBand> SearchPayBands(string? name);
    IEnumerable<LookupItem> LookupItem(LookupItemsEnum? item);
    IEnumerable<string> GetLookupItems();
}
