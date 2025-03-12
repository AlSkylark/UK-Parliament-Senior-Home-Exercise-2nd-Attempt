using FluentValidation;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Mappers.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Services.Mappers;

public class EmployeeMapper(
    ILookUpService lookUpService,
    IValidator<Employee> irregularityValidator
)
    : BasePersonMapper<EmployeeViewModel, Employee>(irregularityValidator, lookUpService),
        IEmployeeMapper
{
    private readonly ILookUpService _lookupService = lookUpService;

    public Employee MapForCreate(EmployeeViewModel vm)
    {
        var payBand = _lookupService.SearchPayBands(vm.PayBand ?? "");
        var department = _lookupService.SearchDepartments(vm.Department ?? "");

        var employee = new Employee
        {
            Id = vm.Id ?? 0,
            FirstName = vm.FirstName!,
            LastName = vm.LastName!,
            DoB = vm.DoB,
            Address = vm.Address ?? new(),
            Salary = vm.Salary,
            BankAccount = vm.BankAccount,
            DateJoined = vm.DateJoined ?? DateOnly.FromDateTime(DateTime.Now),
            DateLeft = vm.DateLeft,
            PayBand = payBand.FirstOrDefault(),
            Department = department.FirstOrDefault(),
            ManagerId = vm.ManagerId,
        };

        return employee;
    }
}
