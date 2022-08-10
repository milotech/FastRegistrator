using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Queries.GetStatus;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class RegistrationController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> StartRegistration(StartRegistrationCommand command, CancellationToken cancel) 
        {
            await ExecuteCommand(command, cancel);
            return Ok();
        }

        [HttpGet("{guid}")]
        public async Task<ActionResult<RegistrationStatusResponse>> GetStatus(Guid guid, CancellationToken cancel)
        {
            var getStatusQuery = new GetRegistrationStatusQuery(guid);
            return await ExecuteQuery(getStatusQuery, cancel);
        }
    }
}
