using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.ValueObjects
{
    public class HumanName : BaseValueObject<string>
    {
        public HumanName()
        {
            this.value = null;
        }

        public HumanName(string name)
        {
            this.value = name;
        }

        public void Set(string name)
        {
            this.value = name;
        }

        public string Get()
        {
            return this.value;
        }
    }
}
