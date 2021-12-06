using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts
{
    public class TenantBasedDynamicDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DummyContent> Contents { get; set; }

        public TenantBasedDynamicDbContext(DbContextOptions<TenantBasedDynamicDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

    }
}
