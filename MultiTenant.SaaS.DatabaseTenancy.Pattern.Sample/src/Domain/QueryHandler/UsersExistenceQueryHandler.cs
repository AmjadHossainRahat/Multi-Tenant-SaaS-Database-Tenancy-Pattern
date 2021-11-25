using Kledex.Queries;
using Microsoft.Extensions.Logging;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.QueryHandler
{
    public class UsersExistenceQueryHandler : IQueryHandlerAsync<UsersExistenceQuery, bool>
    {
        private readonly ILogger<UsersExistenceQueryHandler> logger;

        public UsersExistenceQueryHandler(ILogger<UsersExistenceQueryHandler> logger)
        {
            this.logger = logger;
        }

        public async Task<bool> HandleAsync(UsersExistenceQuery query)
        {
            this.logger.LogInformation($"UsersExistanceQueryHandler/HandleAsync START {query.CorrelationId}");
            //TODO: logic goes here

            this.logger.LogInformation($"UsersExistanceQueryHandler/HandleAsync returning false {query.CorrelationId}");
            return false;
        }
    }
}
