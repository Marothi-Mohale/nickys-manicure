using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/testimonials")]
[Produces("application/json")]
public sealed class TestimonialsApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<TestimonialResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<TestimonialResponse>>> GetList(
        [FromQuery] TestimonialQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetTestimonialsAsync(query, cancellationToken));
    }
}
