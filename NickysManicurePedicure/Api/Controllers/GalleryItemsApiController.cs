using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/gallery-items")]
[Produces("application/json")]
public sealed class GalleryItemsApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<GalleryListItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<GalleryListItemResponse>>> GetList(
        [FromQuery] GalleryItemQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetGalleryItemsAsync(query, cancellationToken));
    }
}
