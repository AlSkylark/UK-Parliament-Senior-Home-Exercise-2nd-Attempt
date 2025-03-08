using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Data.Models;

public class Department : BaseEntity, ILookupItem
{
    public required string Name { get; set; }
}
