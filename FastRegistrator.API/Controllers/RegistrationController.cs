using FastRegistrator.ApplicationCore.Commands.StartRegistration;
using FastRegistrator.ApplicationCore.DTOs.RegistrationStatusDTOs;
using FastRegistrator.ApplicationCore.Queries;
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
