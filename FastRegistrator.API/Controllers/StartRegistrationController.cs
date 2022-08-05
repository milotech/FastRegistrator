using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class StartRegistrationController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> StartRegistration(StartRegistrationCommand command, CancellationToken cancel) 
        {
            await ExecuteCommand(command, cancel);
            return Ok();
        }
    }
}
