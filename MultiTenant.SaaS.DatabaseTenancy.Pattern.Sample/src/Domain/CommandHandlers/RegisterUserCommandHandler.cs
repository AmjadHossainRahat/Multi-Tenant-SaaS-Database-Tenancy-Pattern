using Kledex.Commands;
using Microsoft.Extensions.Configuration;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.CommandHandlers
{
    public class RegisterUserCommandHandler : ICommandHandlerAsync<RegisterUserCommand>
    {
        private readonly UserManagementDbContext userManagementDbContext;
        private readonly SharedDbContext sharedDbContext;
        private readonly IConfiguration configuration;
        public RegisterUserCommandHandler(UserManagementDbContext userManagementDbContext, SharedDbContext sharedDbContext, IConfiguration configuration)
        {
            this.userManagementDbContext = userManagementDbContext;
            this.sharedDbContext = sharedDbContext;
            this.configuration = configuration;
        }

        public async Task<CommandResponse> HandleAsync(RegisterUserCommand command)
        {
            await this.userManagementDbContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            CommandResponse commandResponse = new CommandResponse();
            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                CreationDateTime = DateTime.UtcNow,
                Contents = new List<DummyContent>() { },
                IsArchived = false,
                IsPaidUser = false,
                TenantId = Guid.NewGuid(),  // assuming each user having a different tenant!!
                LastUpdateDateTime = DateTime.UtcNow,
            };

            UserAndDatabaseNameMapping userDatabaseMapping = new UserAndDatabaseNameMapping
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TenantId = user.TenantId,
                Email = user.Email,
                DatabaseName = this.configuration.GetValue<string>("Cosmos:SharedDatabaseName"), // unpaid user goes to shared database
                IsArchived = false,
                CreationDateTime = DateTime.UtcNow,
                LastUpdateDateTime = DateTime.UtcNow,
            };

            await this.userManagementDbContext.UserDatabaseMappings.AddAsync(userDatabaseMapping).ConfigureAwait(false);
            await this.userManagementDbContext.SaveChangesAsync().ConfigureAwait(false);


            await this.sharedDbContext.Users.AddAsync(user).ConfigureAwait(false);
            await this.sharedDbContext.SaveChangesAsync().ConfigureAwait(false);

            commandResponse.Result = user;

            return commandResponse;
        }
    }
}
