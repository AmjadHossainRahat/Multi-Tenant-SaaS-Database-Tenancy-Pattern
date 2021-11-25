using FluentValidation;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Validators
{
    public class SubscribeCommandValidator : AbstractValidator<SubscribeCommand>
    {
        public SubscribeCommandValidator()
        {
            RuleFor(p => p.CorrelationId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("CorrelationId is required");

            RuleFor(p => p.UserId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("UserId is required");

            RuleFor(p => p.Amount)
                .NotEmpty()
                .GreaterThan(decimal.Zero)
                .WithMessage("Amount must be greater than zero");
        }
    }
}
