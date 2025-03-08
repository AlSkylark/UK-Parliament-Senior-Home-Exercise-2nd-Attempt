namespace UKParliament.CodeTest.Data.HATEOAS.Interfaces;

public interface IResourceCollection<T>
{
    public IEnumerable<T> Results { get; }
    public Pagination Pagination { get; }
}
