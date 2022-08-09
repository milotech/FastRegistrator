using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Queries.GetStatus;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class RegistrationController : ApiControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger) 
        {
            _logger = logger;
        }
        
        [HttpPost]
        [Route("registration")]
        public async Task<ActionResult> StartRegistration(StartRegistrationCommand command, CancellationToken cancel) 
        {
            await ExecuteCommand(command, cancel);
            return Ok();
        }

        [HttpGet]
        [Route("registration/{guid}")]
        public async Task<ActionResult<RegistrationStatusResponse>> GetStatus(Guid guid, CancellationToken cancel)
        {
            var getStatusQuery = new GetStatusQuery(guid);

            try
            {
                return await ExecuteQuery(getStatusQuery, cancel);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }
    }
}
