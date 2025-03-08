namespace UKParliament.CodeTest.Data.HATEOAS;

public class Pagination
{
    public int Total { get; set; }
    public int PerPage { get; set; }
    public int CurrentPage { get; set; }
    public int FinalPage { get; set; }
    public string? FirstPageUrl { get; set; }
    public string? FinalPageUrl { get; set; }
    public string? NextPageUrl { get; set; }
    public string? PrevPageUrl { get; set; }
    public string? Path { get; set; }
    public int From { get; set; }
    public int To { get; set; }
}
