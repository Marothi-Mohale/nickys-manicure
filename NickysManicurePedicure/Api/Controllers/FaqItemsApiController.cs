using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Contracts.Common;
using NickysManicurePedicure.Contracts.Requests;
using NickysManicurePedicure.Contracts.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/faqs")]
[Produces("application/json")]
public sealed class FaqItemsApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<FaqItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<FaqItemResponse>>> GetList(
        [FromQuery] FaqQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetFaqItemsAsync(query, cancellationToken));
    }
}
