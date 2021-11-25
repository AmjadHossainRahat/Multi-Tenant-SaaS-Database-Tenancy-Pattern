using Kledex.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.CommandHandlers
{
    public class RegisterUserCommandHandler : ICommandHandlerAsync<RegisterUserCommand>
    {
        public Task<CommandResponse> HandleAsync(RegisterUserCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
