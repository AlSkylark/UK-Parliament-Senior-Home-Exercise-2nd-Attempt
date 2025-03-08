using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data.ViewModels;

public class EmployeeViewModel : BaseViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmployeeType { get; set; }
    public DateOnly? DoB { get; set; }

    public string? PayBand { get; set; }
    public string? Department { get; set; }
    public decimal? Salary { get; set; }
    public string? BankAccount { get; set; }

    public DateOnly? DateJoined { get; set; }
    public DateOnly? DateLeft { get; set; }

    public int? ManagerId { get; set; }
    public ShortManagerViewModel? Manager { get; set; }

    public Address? Address { get; set; }

    public bool Inactive { get; set; }
    public bool HasManager { get; set; }
    public bool IsManager { get; set; }
    public bool HasIrregularities { get; set; }
}
