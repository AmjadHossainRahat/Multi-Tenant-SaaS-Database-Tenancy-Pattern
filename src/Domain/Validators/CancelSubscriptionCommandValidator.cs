using FluentValidation;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Validators
{
    public class CancelSubscriptionCommandValidator : AbstractValidator<CancelSubscriptionCommand>
    {
        public CancelSubscriptionCommandValidator()
        {
            RuleFor(p => p.CorrelationId)
                .NotEqual(Guid.Empty)
                .WithMessage("CorrelationId is required");

            RuleFor(p => p.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("UserId is required");
        }
    }
}
