using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class SetStatusESIAApprovedController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> SetStatus(SetStatusESIAApprovedCommand command, CancellationToken cancel) 
        {
            await ExecuteCommand(command, cancel);
            return Ok();
        }
    }
}
