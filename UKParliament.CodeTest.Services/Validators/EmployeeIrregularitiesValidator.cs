using FluentValidation;
using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Services.Validators;

public class EmployeeIrregularitiesValidator : AbstractValidator<Employee>
{
    public EmployeeIrregularitiesValidator()
    {
        RuleFor(e => e.BankAccount).NotEmpty().When(e => e.Salary != 0);
        RuleFor(e => e.PayBand).NotEmpty().When(e => e.Salary != 0);
        RuleFor(e => e.Salary).NotEqual(0).When(e => e.PayBand != null);
        RuleFor(e => e.Salary).NotEqual(0).When(e => e.BankAccount != null);
        RuleFor(e => e.ManagerId).NotNull().When(e => e.EmployeeType == EmployeeTypeEnum.Employee);
    }
}
