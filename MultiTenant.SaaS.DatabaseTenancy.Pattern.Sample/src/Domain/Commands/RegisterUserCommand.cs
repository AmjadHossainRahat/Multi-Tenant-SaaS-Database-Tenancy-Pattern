using Kledex.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands
{
    public class RegisterUserCommand : Command
    {
        public Guid CorrelationId { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
