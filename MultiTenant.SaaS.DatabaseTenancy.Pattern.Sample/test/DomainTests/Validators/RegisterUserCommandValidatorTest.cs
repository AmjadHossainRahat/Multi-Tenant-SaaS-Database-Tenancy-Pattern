using FluentValidation.Results;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DomainTests.Validators
{
    public class RegisterUserCommandValidatorTest
    {
        private RegisterUserCommandValidator validator;

        public RegisterUserCommandValidatorTest()
        {
            this.validator = new RegisterUserCommandValidator();
        }

        [Fact]
        public async Task Fails_When_CorrelationId_Is_Empty()
        {
            RegisterUserCommand command = new RegisterUserCommand()
            {
                CorrelationId = Guid.Empty,
                Email = "test@mail.com",
            };

            ValidationResult expected = new ValidationResult();
            ValidationFailure failure = new ValidationFailure("CorrelationId", "CorrelationId is required");
            expected.Errors.Add(failure);

            ValidationResult result = await this.validator.ValidateAsync(command).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result.Errors);
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count > 0);
            Assert.Equal(expected.Errors.First().ErrorMessage, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Fails_When_Email_Is_Empty()
        {
            RegisterUserCommand command = new RegisterUserCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Email = string.Empty,
            };

            ValidationResult expected = new ValidationResult();
            ValidationFailure failure = new ValidationFailure("Email", "Email is required");
            expected.Errors.Add(failure);

            ValidationResult result = await this.validator.ValidateAsync(command).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result.Errors);
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count > 0);
            Assert.Equal(expected.Errors.First().ErrorMessage, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Fails_When_Email_IsNot_Valid()
        {
            RegisterUserCommand command = new RegisterUserCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Email = "abc",
            };

            ValidationResult expected = new ValidationResult();
            ValidationFailure failure = new ValidationFailure("Email", "Email is not valid");
            expected.Errors.Add(failure);

            ValidationResult result = await this.validator.ValidateAsync(command).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result.Errors);
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count > 0);
            Assert.Equal(expected.Errors.First().ErrorMessage, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Passes()
        {
            RegisterUserCommand command = new RegisterUserCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Email = "test@mail.com",
            };

            ValidationResult result = await this.validator.ValidateAsync(command).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.Count == 0);
            Assert.True(result.IsValid);
        }
    }
}
