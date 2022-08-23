using FastRegistrator.ApplicationCore.Commands.AccountOpened;
using FastRegistrator.ApplicationCore.Commands.StartRegistration;
using FastRegistrator.ApplicationCore.DTOs.RegistrationStatusDTOs;
using FastRegistrator.ApplicationCore.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FastRegistrator.API.Controllers
{
    public class RegistrationController : ApiControllerBase
    {
        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(RegistrationStatusResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<RegistrationStatusResponse>> GetStatus(Guid guid, CancellationToken cancel)
        {
            var getStatusQuery = new GetRegistrationStatusQuery(guid);
            return await ExecuteQuery(getStatusQuery, cancel);
        }

        [HttpPost("start")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> StartRegistration(StartRegistrationCommand command, CancellationToken cancel)
        {
            await ExecuteCommand(command, cancel);
            return Ok();
        }

        [HttpPost("complete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> CompleteRegistration(AccountOpenedCommand command, CancellationToken cancel)
        {
            await ExecuteCommand(command, cancel);
            return Ok();
        }
    }
}
