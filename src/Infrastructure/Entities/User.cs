using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities
{
    public class User : BaseEntity
    {
        public Guid TenantId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsPaidUser { get; set; }

        public ICollection<DummyContent> Contents { get; set; }
    }
}
