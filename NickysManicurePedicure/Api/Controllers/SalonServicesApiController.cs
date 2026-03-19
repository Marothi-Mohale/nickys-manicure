using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/services")]
[Produces("application/json")]
public sealed class SalonServicesApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<SalonServiceResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<SalonServiceResponse>>> GetList(
        [FromQuery] ServiceCatalogQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetServicesAsync(query, cancellationToken));
    }
}
