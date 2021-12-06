using Microsoft.EntityFrameworkCore;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts
{
    public class SharedDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
