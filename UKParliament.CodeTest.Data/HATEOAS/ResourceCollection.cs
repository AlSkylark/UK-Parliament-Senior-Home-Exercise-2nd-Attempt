using UKParliament.CodeTest.Data.HATEOAS.Interfaces;

namespace UKParliament.CodeTest.Data.HATEOAS;

public class ResourceCollection<T> : IResourceCollection<T>
{
    public required IEnumerable<T> Results { get; init; }
    public required Pagination Pagination { get; init; }
}
