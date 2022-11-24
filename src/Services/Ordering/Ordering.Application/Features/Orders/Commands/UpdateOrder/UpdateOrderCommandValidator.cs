using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator: AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("UserName mustn't be empty")
                .NotNull()
                .MaximumLength(50).WithMessage("Name shouldn't exceed 50 characters");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("Email mustn't be empty");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("Total Price mustn't be empty")
                .GreaterThan(0).WithMessage("Total Price must be greater that zero");
        }
    }
}
