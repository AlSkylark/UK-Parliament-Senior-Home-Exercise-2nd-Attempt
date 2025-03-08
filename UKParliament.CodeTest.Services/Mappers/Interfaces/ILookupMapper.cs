using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.Mappers.Interfaces;

public interface ILookupMapper
{
    LookupItem MapToSimple(ILookupItem item);
    LookupItem MapFromString(string item);
}
