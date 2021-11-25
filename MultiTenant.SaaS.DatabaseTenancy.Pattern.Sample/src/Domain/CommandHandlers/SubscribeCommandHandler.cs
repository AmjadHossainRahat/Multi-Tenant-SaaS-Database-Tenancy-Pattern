using Kledex.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.CommandHandlers
{
    public class SubscribeCommandHandler : ICommandHandlerAsync<SubscribeCommand>
    {
        public async Task<CommandResponse> HandleAsync(SubscribeCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
