using FluentValidation;
using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Services.Validators;

public class EmployeeIrregularitiesValidator : AbstractValidator<Employee>
{
    public EmployeeIrregularitiesValidator(IValidator<Address?> addressValidator)
    {
        RuleFor(e => e.DoB).NotNull().WithMessage("A date of birth should be assigned.");
        RuleFor(e => e.BankAccount)
            .NotEmpty()
            .When(e => e.Salary != 0)
            .WithMessage("A bank account should be added if a salary has been assigned.");
        RuleFor(e => e.Salary)
            .NotEqual(0)
            .When(e => e.BankAccount != null)
            .WithMessage("A salary should be assigned if a bank account has been added.");
        RuleFor(e => e.Address).SetValidator(_ => addressValidator);
        RuleFor(e => e.ManagerId)
            .NotNull()
            .When(e => e.EmployeeType == EmployeeTypeEnum.Employee)
            .WithMessage("A manager should be assigned.");
    }
}
