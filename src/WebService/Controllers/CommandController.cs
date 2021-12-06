using Kledex;
using Kledex.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Queries;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.WebService.Controllers
{
    //[Produces("application/json")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> logger;
        private readonly IDispatcher dispatcher;

        public CommandController(ILogger<CommandController> logger, IDispatcher dispatcher)
        {
            this.logger = logger;
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Ping()
        {
            ObjectResult result = this.Ok("Pong");

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            this.logger.LogInformation($"Command/Register START {command.CorrelationId}");

            UsersExistenceQuery usersExistenceQuery = new UsersExistenceQuery
            {
                CorrelationId = command.CorrelationId,
                Email = command.Email,
            };

            this.logger.LogInformation("Command/Register Going to validate users existence");
            bool userExist = await this.dispatcher.GetResultAsync(usersExistenceQuery).ConfigureAwait(false);
            if (userExist)
            {
                string message = "Email already used";
                this.logger.LogInformation($"Command/Register {message} {command.CorrelationId}");
                return this.BadRequest(message);
            }

            this.logger.LogInformation("Command/Register going to send command");
            object response = await this.dispatcher.SendAsync<object>(command).ConfigureAwait(false);

            this.logger.LogInformation($"Command/Register Returning Accepted {command.CorrelationId}");
            return this.Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeCommand command)
        {
            this.logger.LogInformation($"Command/Subscribe START {command.CorrelationId}");

            UsersExistenceQuery usersExistanceQuery = new UsersExistenceQuery
            {
                CorrelationId = command.CorrelationId,
                UserId = command.Id,
            };

            this.logger.LogInformation("Going to validate users existence");
            bool userExist = await this.dispatcher.GetResultAsync(usersExistanceQuery).ConfigureAwait(false);
            if (!userExist)
            {
                string message = "User does not exist with this Id";
                this.logger.LogInformation($"Command/Subscribe {message} {command.CorrelationId}");
                return this.BadRequest(message);
            }

            this.logger.LogInformation("Command/Subscribe going to send command");
            object response = await this.dispatcher.SendAsync<object>(command).ConfigureAwait(false);

            this.logger.LogInformation($"Command/Subscribe Returning Accepted {command.CorrelationId}");
            return this.Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscribe([FromBody] CancelSubscriptionCommand command)
        {
            this.logger.LogInformation($"Command/Unsubscribe START {command.CorrelationId}");

            UsersExistenceQuery usersExistanceQuery = new UsersExistenceQuery
            {
                CorrelationId = command.CorrelationId,
                UserId = command.Id,
            };

            this.logger.LogInformation("Going to validate users existence");
            bool userExist = await this.dispatcher.GetResultAsync(usersExistanceQuery).ConfigureAwait(false);
            if (!userExist)
            {
                string message = "User does not exist with this Id";
                this.logger.LogInformation($"Command/Unsubscribe {message} {command.CorrelationId}");
                return this.BadRequest(message);
            }

            this.logger.LogInformation("Command/Unsubscribe going to send command");
            object response = await this.dispatcher.SendAsync<object>(command).ConfigureAwait(false);

            this.logger.LogInformation($"Command/Unsubscribe Returning Accepted {command.CorrelationId}");
            return this.Ok(response);
        }
    }
}
