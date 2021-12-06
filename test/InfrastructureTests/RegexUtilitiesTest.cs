using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace InfrastructureTests
{
    public class RegexUtilitiesTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("abc")]
        [InlineData("abc@")]
        [InlineData("abc.")]
        [InlineData("abc.m")]
        [InlineData("abc@m")]
        [InlineData("abc@m.")]
        public void IsValidEmail_Returns_False(string email)
        {
            bool expected = false;

            bool result = RegexUtilities.IsValidEmail(email);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("abc@m.c")]
        [InlineData("abc@m.com")]
        [InlineData("test@mail.com")]
        public void IsValidEmail_Returns_True(string email)
        {
            bool expected = true;

            bool result = RegexUtilities.IsValidEmail(email);

            Assert.Equal(expected, result);
        }
    }
}
