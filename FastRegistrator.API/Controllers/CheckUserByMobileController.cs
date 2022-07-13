using FastRegistrator.ApplicationCore.Commands.CheckUserByMobilePhone;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class CheckUserByMobilePhoneController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<bool>> Check(CheckUserByMobilePhoneCommand command) 
        {
            return await Mediator.Send(command);
        }
    }
}
