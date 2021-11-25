using FluentValidation;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(p => p.CorrelationId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("CorrelationId is required");

            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage("Email is required");

            RuleFor(p => p.Email)
                .Must(RegexUtilities.IsValidEmail)
                .WithMessage("Email is not valid");
        }
    }
}
