using FluentValidation;
using System;
using System.Linq;
using Validators.Polish;
using pl = Validators.Polish;

namespace Vavatech.DotnetCore.Models.Validators
{

    // dotnet add package FluentValidation
    // dotnet add package PolishValidators

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator(PeselValidator validator)
        {
            RuleFor(p => p.LastName).NotEmpty();
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.CreditAmount).InclusiveBetween(1, 1000);
            RuleFor(p => p.Pesel).NotEmpty().Length(11).Must(p=>validator.IsValid(p)).WithMessage("Nieprawidłowy numer PESEL");

            RuleFor(p => p.From).LessThanOrEqualTo(p => p.To).When(p=>p.IsHoliday);
        }

    }
}
