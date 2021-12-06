using Kledex.Commands;
using Microsoft.Extensions.Configuration;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.CommandHandlers
{
    public class CancelSubscriptionCommandHandler : ICommandHandlerAsync<CancelSubscriptionCommand>
    {
        private readonly UserManagementDbContext userManagementDbContext;
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantBasedDynamicDbContext tenantBasedDynamicDbContext;
        private readonly IConfiguration configuration;

        public CancelSubscriptionCommandHandler(
            UserManagementDbContext userManagementDbContext,
            SharedDbContext sharedDbContext,
            TenantBasedDynamicDbContext tenantBasedDynamicDbContext,
            IConfiguration configuration)
        {
            this.userManagementDbContext = userManagementDbContext;
            this.sharedDbContext = sharedDbContext;
            this.tenantBasedDynamicDbContext = tenantBasedDynamicDbContext;
            this.configuration = configuration;
        }

        public async Task<CommandResponse> HandleAsync(CancelSubscriptionCommand command)
        {
            CommandResponse response = new CommandResponse();

            UserAndDatabaseNameMapping mapping = this.userManagementDbContext.UserDatabaseMappings.Where(p => p.UserId == command.Id).FirstOrDefault();
            if (mapping == null)
            {
                response.Result = new { StatusCode = HttpStatusCode.BadRequest, Message = "User does not exist" };
            }

            User user = await this.tenantBasedDynamicDbContext.Users.FindAsync(command.Id).ConfigureAwait(false);
            if (user == null)
            {
                response.Result = new { StatusCode = HttpStatusCode.BadRequest, Message = "User does not have paid subscription" };
            }

            await this.ChangeDatabaseOfTheUser(user, mapping).ConfigureAwait(false);

            return response;
        }

        private async Task ChangeDatabaseOfTheUser(User user, UserAndDatabaseNameMapping mapping)
        {
            //TODO: use event->command to do following tasks for reducing responsibility of this class
            user.IsPaidUser = false;
            user.LastUpdateDateTime = DateTime.UtcNow;

            // transferring the user from shared database
            await this.sharedDbContext.Users.AddAsync(user).ConfigureAwait(false);
            await this.sharedDbContext.SaveChangesAsync().ConfigureAwait(false);

            mapping.DatabaseName = this.configuration.GetValue<string>("Cosmos:SharedDatabaseName");
            mapping.LastUpdateDateTime = DateTime.UtcNow;
            this.userManagementDbContext.Update(mapping);
            await this.userManagementDbContext.SaveChangesAsync().ConfigureAwait(false);

            // Clearing tenant based database
            this.tenantBasedDynamicDbContext.Users.Remove(user);
            await this.tenantBasedDynamicDbContext.SaveChangesAsync().ConfigureAwait(false);
            await tenantBasedDynamicDbContext.Database.EnsureDeletedAsync().ConfigureAwait(false);
        }
    }
}
