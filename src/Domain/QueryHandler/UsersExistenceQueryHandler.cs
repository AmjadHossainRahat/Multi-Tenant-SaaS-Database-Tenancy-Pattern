using Kledex.Queries;
using Microsoft.Extensions.Logging;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Queries;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.QueryHandler
{
    public class UsersExistenceQueryHandler : IQueryHandlerAsync<UsersExistenceQuery, bool>
    {
        private readonly ILogger<UsersExistenceQueryHandler> logger;
        private readonly UserManagementDbContext userManagementDbContext;

        public UsersExistenceQueryHandler(ILogger<UsersExistenceQueryHandler> logger, UserManagementDbContext userManagementDbContext)
        {
            this.logger = logger;
            this.userManagementDbContext = userManagementDbContext;
        }

        public async Task<bool> HandleAsync(UsersExistenceQuery query)
        {
            this.logger.LogInformation($"UsersExistanceQueryHandler/HandleAsync START {query.CorrelationId}");

            Expression<Func<UserAndDatabaseNameMapping, bool>> exp = null;

            if (query.UserId != null && query.UserId != Guid.Empty)
            {
                exp = (p => p.UserId == query.UserId);
            }
            else if (!string.IsNullOrEmpty(query.Email))
            {
                exp = (p => p.Email == query.Email);
            }

            if (exp == null)
            {
                throw new Exception("Query fro UserDatabaseMappings was invalid");
            }

            UserAndDatabaseNameMapping mapping = this.userManagementDbContext.UserDatabaseMappings.Where(exp).FirstOrDefault();

            if (mapping != null)
            {
                this.logger.LogInformation($"UsersExistanceQueryHandler/HandleAsync returning true {query.CorrelationId}");
                return true;
            }

            this.logger.LogInformation($"UsersExistanceQueryHandler/HandleAsync returning false {query.CorrelationId}");
            return false;
        }
    }
}
