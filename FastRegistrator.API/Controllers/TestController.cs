using FastRegistrator.ApplicationCore.Commands.Tests;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class TestController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Check(string name, CancellationToken cancel)
        {
            await ExecuteCommand(new TestEventsCommand("89990000000", name, "Ivanov"), cancel);

            return Ok();
        }

        [HttpGet("/queue")]
        public ActionResult TestQueuedCommands(string name, CancellationToken cancel)
        {

            for (int i = 0; i < 100; i++)
            {
                var cmd = new TestRunFromQueueCommand($"{name} # {i}");
                _ = ExecuteCommand(cmd, cancel);
            }

            return Ok();
        }
    }
}
