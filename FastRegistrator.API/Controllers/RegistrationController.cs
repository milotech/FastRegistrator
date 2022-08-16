using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Queries.GetStatus;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class RegistrationController : ApiControllerBase
    {
        [HttpGet("{guid}")]
        public async Task<ActionResult<RegistrationStatusResponse>> GetStatus(Guid guid, CancellationToken cancel)
        {
            var getStatusQuery = new GetRegistrationStatusQuery(guid);
            return await ExecuteQuery(getStatusQuery, cancel);
        }

        [HttpPost("start")]
        public async Task<ActionResult> StartRegistration(StartRegistrationCommand command, CancellationToken cancel) 
        {
            await ExecuteCommand(command, cancel);
            return Ok();
        }
    }
}
