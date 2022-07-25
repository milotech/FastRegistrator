using FastRegistrator.ApplicationCore.Commands.SetStatusESIANotApproved;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class SetStatusESIANotApprovedController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> SetStatus(SetStatusESIANotApprovedCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}