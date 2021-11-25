using Kledex.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Queries
{
    public class UsersExistenceQuery : IQuery<bool>
    {
        public Guid CorrelationId { get; set; }

        public Guid? UserId { get; set; }

        public string Email { get; set; }
    }
}
