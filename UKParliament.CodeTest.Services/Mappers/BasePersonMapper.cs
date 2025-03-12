using FluentValidation;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Helpers;
using UKParliament.CodeTest.Services.Mappers.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Services.Mappers;

public class BasePersonMapper<Tview, Tbase>(
    IValidator<Employee> irregularityValidator,
    ILookUpService lookUpService
) : IBasePersonMapper<Tview, Tbase>
    where Tview : EmployeeViewModel, new()
    where Tbase : Employee, new()
{
    public virtual Tview Map(Tbase person)
    {
        var irregularities = irregularityValidator.Validate(person);
        var vm = new Tview
        {
            Id = person.Id,
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DoB = person.DoB,
            Department = person.Department?.Name,
            PayBand = person.PayBand?.Name,
            EmployeeType = person.EmployeeType.GetDescription(),
            Address = person.Address,
            Salary = person.Salary,
            BankAccount = person.BankAccount,
            DateJoined = person.DateJoined,
            DateLeft = person.DateLeft,

            ManagerId = person.ManagerId,

            Inactive = person.DateLeft is not null,
            HasManager = person.ManagerId > 0,
            IsManager = person.EmployeeType == EmployeeTypeEnum.Manager,
            Irregularities = irregularities,
            HasIrregularities = !irregularities?.IsValid ?? false,
        };

        if (person.Manager is not null)
        {
            vm.Manager = MapManager(person.Manager);
        }

        return vm;
    }

    public ShortManagerViewModel MapManager(Employee manager)
    {
        return new ShortManagerViewModel
        {
            FirstName = manager.FirstName,
            LastName = manager.LastName,
            Id = manager.Id,
        };
    }

    public Tbase MapForSave(EmployeeViewModel vm, Tbase existing)
    {
        var payBand = vm.PayBand is null ? null : lookUpService.SearchPayBands(vm.PayBand);
        var department = vm.Department is null
            ? null
            : lookUpService.SearchDepartments(vm.Department);

        var employee = new Tbase
        {
            Id = vm.Id ?? existing.Id,
            FirstName = vm.FirstName!,
            LastName = vm.LastName!,
            DoB = vm.DoB,
            Address = vm.Address,
            Salary = vm.Salary,
            BankAccount = vm.BankAccount,
            DateJoined = vm.DateJoined ?? existing.DateJoined,
            DateLeft = vm.DateLeft,
            PayBandId = payBand?.FirstOrDefault()?.Id,
            DepartmentId = department?.FirstOrDefault()?.Id,
            ManagerId = vm.ManagerId,

            CreatedAt = existing.CreatedAt,
        };

        return employee;
    }
}
