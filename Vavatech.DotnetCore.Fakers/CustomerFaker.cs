using Bogus;
using Bogus.Extensions;
using Bogus.Extensions.Poland;
using System;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.Fakers
{
    // dotnet add package Bogus
    // dotnet add package Sulmar.Bogus.Extensions.Poland
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker()
        {
            var lorem = new Bogus.DataSets.Lorem(locale: "pl");

            RuleFor(p => p.Id, f => f.IndexFaker);
            RuleFor(p => p.FirstName, f => f.Person.FirstName);
            RuleFor(p => p.LastName, f => f.Person.LastName);
            RuleFor(p => p.Gender, f => (Gender) f.Person.Gender);
            RuleFor(p => p.Email, (f, customer) => $"{customer.FirstName}.{customer.LastName}@domain.com"); // firstname.lastname@domain.com
            RuleFor(p => p.Description, f => lorem.Sentence(1));
            RuleFor(p => p.Birthday, f => f.Person.DateOfBirth);
            RuleFor(p => p.Type, f => f.PickRandom<CustomerType>());
            RuleFor(p => p.CreditAmount, f => f.Random.Decimal(1m, 1000m).OrNull(f, 0.3f));
            RuleFor(p => p.IsRemoved, f => f.Random.Bool(0.2f));
            RuleFor(p => p.Pesel, f => f.Person.Pesel());

            RuleFor(p => p.Username, f => f.Person.UserName);
            RuleFor(p => p.HashedPassword, f => "12345");
        }
    }
}
