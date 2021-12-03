using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Entities
{
    public class PaidUser
    {
        public Guid Id { get; set; }

        public Email Email { get; set; }

        public HumanName FirstName { get; set; }

        public HumanName LastName { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public Decimal PaidAmount { get; set; }
    }
}
