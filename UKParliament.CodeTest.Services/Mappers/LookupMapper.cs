using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Mappers.Interfaces;

namespace UKParliament.CodeTest.Services.Mappers;

public class LookupMapper : ILookupMapper
{
    public LookupItem MapFromString(string item)
    {
        return new LookupItem { Name = item };
    }

    public LookupItem MapToSimple(ILookupItem item)
    {
        return new LookupItem { Id = item.Id, Name = item.Name };
    }
}
