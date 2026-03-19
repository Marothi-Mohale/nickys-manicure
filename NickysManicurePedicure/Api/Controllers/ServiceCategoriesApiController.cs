using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/service-categories")]
[Produces("application/json")]
public sealed class ServiceCategoriesApiController(IPublicSalonApiService publicSalonApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ServiceCategoryListItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ServiceCategoryListItemResponse>>> GetList(
        CancellationToken cancellationToken)
    {
        return Ok(await publicSalonApiService.GetServiceCategoriesAsync(cancellationToken));
    }
}
