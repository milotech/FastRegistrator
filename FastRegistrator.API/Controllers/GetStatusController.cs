using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Queries.GetStatus;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    public class GetStatusController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<GetStatusResponse>> GetStatus(GetStatusQuery query, CancellationToken cancel)
        {
            return await ExecuteQuery(query, cancel);
        }
    }
}