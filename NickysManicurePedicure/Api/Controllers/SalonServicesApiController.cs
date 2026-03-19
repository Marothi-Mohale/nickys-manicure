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
    [ProducesResponseType(typeof(PagedResponse<ServiceListItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ServiceListItemResponse>>> GetList(
        [FromQuery] ServiceCatalogQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetServicesAsync(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServiceDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceDetailResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var service = await publicSalonApiService.GetServiceByIdAsync(id, cancellationToken);
        return service is null ? NotFound() : Ok(service);
    }
}
