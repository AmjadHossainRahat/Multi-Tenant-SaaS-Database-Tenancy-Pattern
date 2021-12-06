using Kledex.Commands;
using Microsoft.AspNetCore.Http;
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
    public class SubscribeCommandHandler : ICommandHandlerAsync<SubscribeCommand>
    {
        private readonly UserManagementDbContext userManagementDbContext;
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantBasedDynamicDbContext tenantBasedDynamicDbContext;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SubscribeCommandHandler(
            UserManagementDbContext userManagementDbContext,
            SharedDbContext sharedDbContext,
            TenantBasedDynamicDbContext tenantBasedDynamicDbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userManagementDbContext = userManagementDbContext;
            this.sharedDbContext = sharedDbContext;
            this.tenantBasedDynamicDbContext = tenantBasedDynamicDbContext;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<CommandResponse> HandleAsync(SubscribeCommand command)
        {
            CommandResponse response = new CommandResponse();

            UserAndDatabaseNameMapping mapping = this.userManagementDbContext.UserDatabaseMappings.Where(p => p.UserId == command.Id).FirstOrDefault();
            if (mapping == null)
            {
                response.Result = new { StatusCode = HttpStatusCode.BadRequest, Message = "User does not exist" };
            }

            User user = await this.sharedDbContext.Users.FindAsync(command.Id).ConfigureAwait(false);
            if (user == null)
            {
                response.Result = new { StatusCode = HttpStatusCode.BadRequest, Message = "User does not exist" };
            }

            await this.ChangeDatabaseOfTheUser(user, mapping).ConfigureAwait(false);

            response.Result = new { user.Id, user.TenantId, user.Email, user.FirstName, user.LastName };
            return response;
        }

        private async Task ChangeDatabaseOfTheUser(User user, UserAndDatabaseNameMapping mapping)
        {
            //TODO: use event->command to do following tasks for reducing responsibility of this class
            // delete the user from shared database
            this.sharedDbContext.Users.Remove(user);
            await this.sharedDbContext.SaveChangesAsync().ConfigureAwait(false);

            user.IsPaidUser = true;
            user.LastUpdateDateTime = DateTime.UtcNow;
            user.Contents = SubscribeCommandHandler.GetDummyContents(user);

            string tenantId = this.httpContextAccessor.HttpContext.Request.Headers["Tenant-Id"].First();
            user.TenantId = Guid.Parse(tenantId);

            await this.tenantBasedDynamicDbContext.Users.AddAsync(user).ConfigureAwait(false);
            foreach(DummyContent content in user.Contents)
            {
                await this.tenantBasedDynamicDbContext.Contents.AddAsync(content).ConfigureAwait(false);
            }
            
            await this.tenantBasedDynamicDbContext.SaveChangesAsync().ConfigureAwait(false);

            string databaseNamePrefix = this.configuration.GetValue<string>("Cosmos:DynamicDatabaseNamePrefix");
            mapping.DatabaseName = $"{databaseNamePrefix}{user.TenantId.ToString()}";
            mapping.LastUpdateDateTime = DateTime.UtcNow;
            this.userManagementDbContext.Update(mapping);
            await this.userManagementDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static List<DummyContent> GetDummyContents(User user)
        {
            List<DummyContent> contents = new List<DummyContent>
            {
                new DummyContent()
                {
                    CreationDateTime = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    User = user,
                    UserId = user.Id,
                    Item1 = "item-1",
                    Item2 = "item-2",
                    Item3 = "item-3",
                    IsArchived = false,
                    LastUpdateDateTime = DateTime.UtcNow,
                },
                new DummyContent()
                {
                    CreationDateTime = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    User = user,
                    UserId = user.Id,
                    Item1 = "item-1",
                    Item2 = "item-2",
                    Item3 = "item-3",
                    IsArchived = false,
                    LastUpdateDateTime = DateTime.UtcNow,
                },
            };

            return contents;
        }
    }
}
