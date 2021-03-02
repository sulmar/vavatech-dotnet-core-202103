using FluentValidation;

namespace Vavatech.DotnetCore.Models.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(p => p.Details).Must(details => details.Count>2);
        }

       
    }
}
