using FastRegistrator.ApplicationCore.Commands.Tests;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class TestController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Check(Guid id, string name, CancellationToken cancel)
        {
            await ExecuteCommand(new TestEventsCommand(id, "89990000000", name, "Ivanov"), cancel);

            return Ok();
        }

        [HttpGet("/queue")]
        public async Task<ActionResult> TestQueuedCommands(string name, CancellationToken cancel)
        {

            for (int i = 0; i < 40; i++)
            {
                var cmd = new TestRunFromQueueCommand($"{name} # {i}");
                _ = ExecuteCommand(cmd, cancel);
            }
            //var checkCommand = new TestRunFromQueueCommand($"{name} # CHECK COMMAND");
            //await ExecuteCommand(checkCommand, cancel);

            return Ok();
        }
    }
}
