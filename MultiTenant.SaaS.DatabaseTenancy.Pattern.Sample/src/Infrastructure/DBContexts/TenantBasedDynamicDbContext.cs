using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts
{
    public class TenantBasedDynamicDbContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;

        public TenantBasedDynamicDbContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder = this.GetDbContextOptionsBuilder();
            base.OnConfiguring(optionsBuilder);

            this.Database.EnsureCreated();
        }

        private DbContextOptionsBuilder GetDbContextOptionsBuilder()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();

            var httpContext = this.httpContextAccessor.HttpContext;

            string accountEndpoint = this.configuration.GetValue<string>("Cosmos:AccountEndpoint");
            string accountKey = this.configuration.GetValue<string>("Cosmos:AccountKey");
            string dynamicDatabaseNamePrefix = this.configuration.GetValue<string>("Cosmos:DynamicDatabaseNamePrefix");

            if (httpContext == null)
            {
                throw new Exception("HttpContext not found");
            }

            string customHttpHeader = "tenant";

            if (!httpContext.Request.Headers.ContainsKey(customHttpHeader))
            {
                throw new Exception("custom header 'tenant' was not found");
            }

            string tenant = httpContext.Request.Headers[customHttpHeader];

            string databaseName = $"{dynamicDatabaseNamePrefix}{tenant}";

            optionsBuilder.UseCosmos(accountEndpoint, accountKey, databaseName);

            return optionsBuilder;
        }

        public void Reset()
        {
            DbContextOptionsBuilder optionsBuilder = this.GetDbContextOptionsBuilder();
            base.OnConfiguring(optionsBuilder);

            this.Database.EnsureCreated();
        }

        public void Reset(string tenantId)
        {
            DbContextOptionsBuilder optionsBuilder = this.GetDbContextOptionsBuilder();
            base.OnConfiguring(optionsBuilder);

            this.Database.EnsureCreated();
        }
    }
}
