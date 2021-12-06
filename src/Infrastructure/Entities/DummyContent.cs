using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities
{
    public class DummyContent : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public string Item1 { get; set; }

        public string Item2 { get; set; }

        public string Item3 { get; set; }
    }
}
