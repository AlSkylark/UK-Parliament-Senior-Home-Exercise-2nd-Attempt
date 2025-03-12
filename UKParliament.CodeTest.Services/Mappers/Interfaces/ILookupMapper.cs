using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.Mappers.Interfaces;

public interface ILookupMapper
{
    LookupItem MapToSimple(ILookupItem item);
    LookupItem MapFromString(string item);
    LookupItem MapFromEmployee(Employee employee);
}
