using Kledex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Queries;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.WebService.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebService.Tests
{
    public class CommandControllerTest
    {
        private CommandController commandController;

        private Mock<ILogger<CommandController>> logger;
        private Mock<IDispatcher> dispatcher;

        public CommandControllerTest()
        {
            this.logger = new Mock<ILogger<CommandController>>();
            this.dispatcher = new Mock<IDispatcher>();

            this.ResetController();
        }

        private void ResetController()
        {
            this.commandController = new CommandController(this.logger.Object, this.dispatcher.Object);
        }

        [Fact]
        public async Task PingAsync_Returns_OK_Response()
        {
            ObjectResult expected = new OkObjectResult("Pong");

            IActionResult result = await this.commandController.Ping().ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task Register_returns_BadRequest()
        {
            this.dispatcher.Setup(p => p.GetResultAsync<bool>(It.IsAny<UsersExistenceQuery>()))
                .ReturnsAsync(true);

            this.ResetController();

            RegisterUserCommand command = new RegisterUserCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Email = "test@mail.com"
            };

            string message = "Email already used";
            ObjectResult expected = new BadRequestObjectResult(message);

            IActionResult result = await this.commandController.Register(command).ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task Register_returns_Accepted()
        {
            this.dispatcher.Setup(p => p.GetResultAsync<bool>(It.IsAny<UsersExistenceQuery>()))
                .ReturnsAsync(false);

            this.ResetController();

            RegisterUserCommand command = new RegisterUserCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Email = "test@mail.com"
            };

            ObjectResult expected = new AcceptedResult();

            IActionResult result = await this.commandController.Register(command).ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task Subscribe_returns_BadRequest()
        {
            this.dispatcher.Setup(p => p.GetResultAsync<bool>(It.IsAny<UsersExistenceQuery>()))
                .ReturnsAsync(false);

            this.ResetController();

            SubscribeCommand command = new SubscribeCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Amount = 100.00m,
            };

            string message = "User does not exist with this Id";
            ObjectResult expected = new BadRequestObjectResult(message);

            IActionResult result = await this.commandController.Subscribe(command).ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task Subscribe_returns_Accepted()
        {
            this.dispatcher.Setup(p => p.GetResultAsync<bool>(It.IsAny<UsersExistenceQuery>()))
                .ReturnsAsync(true);

            this.ResetController();

            SubscribeCommand command = new SubscribeCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Amount = 100.00m,
            };

            ObjectResult expected = new AcceptedResult();

            IActionResult result = await this.commandController.Subscribe(command).ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task Unsubscribe_returns_BadRequest()
        {
            this.dispatcher.Setup(p => p.GetResultAsync<bool>(It.IsAny<UsersExistenceQuery>()))
                .ReturnsAsync(false);

            this.ResetController();

            CancelSubscriptionCommand command = new CancelSubscriptionCommand()
            {
                CorrelationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };

            string message = "User does not exist with this Id";
            ObjectResult expected = new BadRequestObjectResult(message);

            IActionResult result = await this.commandController.Unsubscribe(command).ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task Unsubscribe_returns_Accepted()
        {
            this.dispatcher.Setup(p => p.GetResultAsync<bool>(It.IsAny<UsersExistenceQuery>()))
                .ReturnsAsync(true);

            this.ResetController();

            CancelSubscriptionCommand command = new CancelSubscriptionCommand()
            {
                CorrelationId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };

            ObjectResult expected = new AcceptedResult();

            IActionResult result = await this.commandController.Unsubscribe(command).ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}
