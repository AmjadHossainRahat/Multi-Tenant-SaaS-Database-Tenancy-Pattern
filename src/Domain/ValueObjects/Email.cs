using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.ValueObjects
{
    public class Email : BaseValueObject<string>
    {
        public Email()
        {
            this.value = null;
        }

        public Email(string email)
        {
            this.value = email;
        }

        public void Set(string email)
        {
            this.value = email;
        }

        public string Get()
        {
            return this.value;
        }
    }
}
