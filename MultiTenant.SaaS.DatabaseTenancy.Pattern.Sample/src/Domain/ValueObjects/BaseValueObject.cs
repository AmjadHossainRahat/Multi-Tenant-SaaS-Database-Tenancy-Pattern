using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.ValueObjects
{
    public abstract class BaseValueObject<T>
    {
        protected T value;
        public virtual bool IsNullOrEmpty()
        {
            if (this.value == null)
            {
                return true;
            }

            if (typeof(T) == typeof(string) && this.value.ToString() == string.Empty)
            {
                return true;
            }
            else if(typeof(T) == typeof(Guid) && Guid.Parse(this.value.ToString()) == Guid.Empty)
            {
                return true;
            }

            return false;
        }

        public virtual T Get()
        {
            return this.value;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
