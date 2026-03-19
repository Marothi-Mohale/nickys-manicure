using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/business-hours")]
[Produces("application/json")]
public sealed class BusinessHoursApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<BusinessHourResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<BusinessHourResponse>>> Get(CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetBusinessHoursAsync(cancellationToken));
    }
}
