using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
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

        public CommandControllerTest()
        {
            this.logger = new Mock<ILogger<CommandController>>();

            this.commandController = new CommandController(this.logger.Object);
        }

        [Fact]
        public async Task PingAsync_Returns_OK_Response()
        {
            ObjectResult expected = new OkObjectResult("Pong");

            IActionResult result = await this.commandController.Ping().ConfigureAwait(false);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}
