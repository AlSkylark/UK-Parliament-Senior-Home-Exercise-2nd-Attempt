using FluentValidation;
using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Services.Validators
{
    class AddressValidator : AbstractValidator<Address?>
    {
        public AddressValidator()
        {
            RuleFor(a => a.Address1).NotNull().WithMessage("Address should not be empty");
            RuleFor(a => a.Address1)
                .NotEmpty()
                .When(a => a?.Address1 is not null)
                .WithMessage("Address should not be empty");

            RuleFor(a => a.Postcode).NotNull().WithMessage("Postcode should not be empty");
            RuleFor(a => a.Postcode)
                .NotEmpty()
                .When(a => a?.Postcode is not null)
                .WithMessage("Postcode should not be empty");
        }
    }
}
