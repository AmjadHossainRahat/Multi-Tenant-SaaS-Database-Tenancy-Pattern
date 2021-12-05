using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities
{
    public class UserAndDatabaseNameMapping : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public string Email { get; set; }

        public string DatabaseName { get; set; }
    }
}
