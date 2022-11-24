using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands
{
    public class CheckOutOrderCommandValidator: AbstractValidator<CheckOutOrderCommand>
    {
        public CheckOutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("Username is required")
                .NotNull()
                .MaximumLength(50).WithMessage("Name mustn't exceed 50 characters");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("Email is required");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("Total Price Can't be Empty")
                .GreaterThan(0).WithMessage("Total Price should be greater than zero");
        }
    }
}
