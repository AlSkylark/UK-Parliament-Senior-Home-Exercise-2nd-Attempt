namespace UKParliament.CodeTest.Data.HATEOAS.Interfaces;

public interface IPaginatable
{
    public int Limit { get; }
    public int Page { get; }
}
