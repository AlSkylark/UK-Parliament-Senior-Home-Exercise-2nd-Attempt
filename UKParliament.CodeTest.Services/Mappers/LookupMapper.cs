using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Mappers.Interfaces;

namespace UKParliament.CodeTest.Services.Mappers;

public class LookupMapper : ILookupMapper
{
    public LookupItem MapFromEmployee(Employee employee)
    {
        return new LookupItem
        {
            Id = employee.Id,
            Name = $"{employee.LastName}, {employee.FirstName}",
        };
    }

    public LookupItem MapFromString(string item)
    {
        return new LookupItem { Name = item };
    }

    public LookupItem MapToSimple(ILookupItem item)
    {
        return new LookupItem { Id = item.Id, Name = item.Name };
    }
}
