using FastRegistrator.ApplicationCore.Commands.CompleteRegistration;
using FastRegistrator.ApplicationCore.Commands.StartRegistration;
using FastRegistrator.ApplicationCore.DTOs.RegistrationStatusDTOs;
using FastRegistrator.ApplicationCore.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FastRegistrator.API.Controllers
{
    public class RegistrationController : ApiControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistrationStatusResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<RegistrationStatusResponse> GetStatus(Guid id, CancellationToken cancel)
        {
            var getStatusQuery = new GetRegistrationStatusQuery(id);
            return await ExecuteQuery(getStatusQuery, cancel);
        }

        [HttpPost("start")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task StartRegistration(StartRegistrationCommand command, CancellationToken cancel)
        {
            await ExecuteCommand(command, cancel);
        }

        [HttpPost("complete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task CompleteRegistration(CompleteRegistrationByICCommand command, CancellationToken cancel)
        {
            await ExecuteCommand(command, cancel);
        }
    }
}
