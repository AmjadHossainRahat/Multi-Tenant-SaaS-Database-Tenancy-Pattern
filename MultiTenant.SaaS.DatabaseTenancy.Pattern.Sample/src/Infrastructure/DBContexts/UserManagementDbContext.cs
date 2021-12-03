using Microsoft.EntityFrameworkCore;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts
{
    public class UserManagementDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {

        }
    }
}
