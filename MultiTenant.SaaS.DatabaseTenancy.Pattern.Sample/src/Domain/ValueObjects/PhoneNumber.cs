using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.ValueObjects
{
    public class PhoneNumber : BaseValueObject<string>
    {
        public PhoneNumber()
        {
            this.value = null;
        }

        public PhoneNumber(string phoneNumber)
        {
            this.value = phoneNumber;
        }

        public void Set(string phoneNumber)
        {
            this.value = phoneNumber;
        }
    }
}
