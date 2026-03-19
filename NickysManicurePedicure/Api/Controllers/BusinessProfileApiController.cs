using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Contracts.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/business/profile")]
[Produces("application/json")]
public sealed class BusinessProfileApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(BusinessProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BusinessProfileResponse>> Get(CancellationToken cancellationToken)
    {
        var profile = await publicSalonApiService.GetBusinessProfileAsync(cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }
}
