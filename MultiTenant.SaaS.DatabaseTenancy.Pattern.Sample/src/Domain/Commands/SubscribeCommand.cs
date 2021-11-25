using Kledex.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands
{
    public class SubscribeCommand : Command
    {
        public Guid CorrelationId { get; set; }

        // overriding parents property type
        public new Guid UserId { get; set; }

        public Decimal Amount { get; set; }
    }
}
