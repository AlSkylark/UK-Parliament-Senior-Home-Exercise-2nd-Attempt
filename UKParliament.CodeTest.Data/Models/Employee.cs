namespace UKParliament.CodeTest.Data.Models;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly? DoB { get; set; }
    public EmployeeTypeEnum EmployeeType { get; set; }
    public string? BankAccount { get; set; }
    public DateOnly DateJoined { get; set; }
    public DateOnly? DateLeft { get; set; }
    public decimal Salary { get; set; }

    public PayBand? PayBand { get; set; }
    public Department? Department { get; set; }

    public Address? Address { get; set; }

    public int? ManagerId { get; set; }
    public Manager? Manager { get; set; }
}
