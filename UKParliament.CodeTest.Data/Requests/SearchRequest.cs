using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.Requests;

public class SearchRequest : IPaginatable
{
    public string? TextSearch { get; set; }
    public EmployeeTypeEnum? EmployeeType { get; set; }
    public string? PayBand { get; set; }
    public string? Department { get; set; }

    public int Limit { get; set; } = 20;
    public int Page { get; set; } = 1;
}
