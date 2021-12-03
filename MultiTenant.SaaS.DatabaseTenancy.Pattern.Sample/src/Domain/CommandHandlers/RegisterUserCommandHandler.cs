using Kledex.Commands;
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
        private UserManagementDbContext userManagementDbContext;
        private SharedDbContext sharedDbContext;
        public RegisterUserCommandHandler(UserManagementDbContext userManagementDbContext, SharedDbContext sharedDbContext)
        {
            this.userManagementDbContext = userManagementDbContext;
            this.sharedDbContext = sharedDbContext;
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
                LastUpdateDateTime = DateTime.UtcNow,
            };

            await this.userManagementDbContext.Users.AddAsync(user).ConfigureAwait(false);
            await this.userManagementDbContext.SaveChangesAsync().ConfigureAwait(false);

            return commandResponse;
        }
    }
}
