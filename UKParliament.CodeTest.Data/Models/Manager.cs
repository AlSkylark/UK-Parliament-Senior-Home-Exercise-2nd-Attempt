namespace UKParliament.CodeTest.Data.Models;

public class Manager : Employee
{
    public IList<Employee> Employees { get; set; } = [];
}
