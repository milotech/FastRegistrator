using FastRegistrator.ApplicationCore.Commands.CheckPersonByPhone;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class CheckUserByMobilePhoneController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<bool>> Check(CheckPersonByPhoneCommand command) 
        {
            return await Mediator.Send(command);
        }
    }
}
