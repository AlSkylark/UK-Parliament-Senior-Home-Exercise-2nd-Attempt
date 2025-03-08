namespace UKParliament.CodeTest.Data.HATEOAS.Interfaces;

public interface IResource<T>
{
    public T Data { get; }
    public IEnumerable<Link> Links { get; }
}
