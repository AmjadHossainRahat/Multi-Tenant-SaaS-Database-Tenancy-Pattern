using Kledex.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands
{
    public class CancelSubscriptionCommand : Command
    {
        public Guid CorrelationId { get; set; }
        
        public Guid Id { get; set; }
    }
}
