using FastRegistrator.ApplicationCore.Commands.Tests;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class TestController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Check(Guid id, string name, int age)
        {
            await Mediator.Send(new TestEventsCommand(id, "89990000000", name, "Ivanov"));

            return Ok();
        }
    }
}
