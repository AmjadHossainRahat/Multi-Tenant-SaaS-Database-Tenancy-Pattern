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
    public class CancelSubscriptionCommandValidatorTest
    {
        private CancelSubscriptionCommandValidator validator;

        public CancelSubscriptionCommandValidatorTest()
        {
            this.validator = new CancelSubscriptionCommandValidator();
        }

        [Fact]
        public async Task Fails_When_CorrelationId_Is_Empty()
        {
            CancelSubscriptionCommand command = new CancelSubscriptionCommand()
            {
                CorrelationId = Guid.Empty,
                UserId = Guid.NewGuid(),
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
            CancelSubscriptionCommand command = new CancelSubscriptionCommand()
            {
                CorrelationId = Guid.NewGuid(),
                UserId = Guid.Empty,
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
        public async Task Passes()
        {
            CancelSubscriptionCommand command = new CancelSubscriptionCommand()
            {
                CorrelationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };

            ValidationResult result = await this.validator.ValidateAsync(command).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.Count == 0);
            Assert.True(result.IsValid);
        }
    }
}
