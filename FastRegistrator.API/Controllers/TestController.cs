using FastRegistrator.ApplicationCore.Commands.Tests;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class TestController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Check(string name, int age)
        {
            await Mediator.Send(new TestEventsCommand(name, age));
            return Ok();
        }
    }
}
