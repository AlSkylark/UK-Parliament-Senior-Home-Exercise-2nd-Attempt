using UKParliament.CodeTest.Data.HATEOAS.Interfaces;

namespace UKParliament.CodeTest.Data.HATEOAS;

public class Resource<T> : IResource<T>
{
    public required T Data { get; set; }
    public required IEnumerable<Link> Links { get; set; }
}
