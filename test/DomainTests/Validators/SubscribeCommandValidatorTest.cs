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
    public class SubscribeCommandValidatorTest
    {
        private SubscribeCommandValidator validator;

        public SubscribeCommandValidatorTest()
        {
            this.validator = new SubscribeCommandValidator();
        }

        [Fact]
        public async Task Fails_When_CorrelationId_Is_Empty()
        {
            SubscribeCommand command = new SubscribeCommand()
            {
                CorrelationId = Guid.Empty,
                Id = Guid.NewGuid(),
                Amount = 100.00m,
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
        public async Task Fails_When_UserId_Is_Empty()
        {
            SubscribeCommand command = new SubscribeCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Id = Guid.Empty,
            };

            ValidationResult expected = new ValidationResult();
            ValidationFailure failure = new ValidationFailure("UserId", "UserId is required");
            expected.Errors.Add(failure);

            ValidationResult result = await this.validator.ValidateAsync(command).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result.Errors);
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count > 0);
            Assert.Equal(expected.Errors.First().ErrorMessage, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Fails_When_Amount_IsNot_Positive()
        {
            SubscribeCommand command = new SubscribeCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Amount = 0.0m
            };

            ValidationResult expected = new ValidationResult();
            ValidationFailure failure = new ValidationFailure("Amount", "Amount must be greater than zero");
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
            SubscribeCommand command = new SubscribeCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Amount = 100.00m,
            };

            ValidationResult result = await this.validator.ValidateAsync(command).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.Count == 0);
            Assert.True(result.IsValid);
        }
    }
}
