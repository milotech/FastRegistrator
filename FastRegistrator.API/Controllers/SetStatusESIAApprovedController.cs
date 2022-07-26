using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class SetStatusESIAApprovedController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> SetStatus(SetStatusESIAApprovedCommand command) 
        {
            return await Mediator.Send(command);
        }
    }
}
