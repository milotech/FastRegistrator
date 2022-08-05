using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class StartRegistrationController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> SetStatus(StartRegistrationCommand command) 
        {
            return await Mediator.Send(command);
        }
    }
}
