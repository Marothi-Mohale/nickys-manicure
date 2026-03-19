using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Contracts.Common;
using NickysManicurePedicure.Contracts.Requests;
using NickysManicurePedicure.Contracts.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/gallery-items")]
[Produces("application/json")]
public sealed class GalleryItemsApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<GalleryItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<GalleryItemResponse>>> GetList(
        [FromQuery] GalleryItemQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetGalleryItemsAsync(query, cancellationToken));
    }
}
