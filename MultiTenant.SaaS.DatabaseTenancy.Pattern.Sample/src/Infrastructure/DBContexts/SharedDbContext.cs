using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts
{
    public class SharedDbContext : DbContext
    {
        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options)
        {

        }
    }
}
