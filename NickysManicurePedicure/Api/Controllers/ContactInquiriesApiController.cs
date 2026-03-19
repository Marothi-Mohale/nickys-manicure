using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/contact-inquiries")]
[Produces("application/json")]
public sealed class ContactInquiriesApiController(IInquiryApiCommandService inquiryApiCommandService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ContactInquiryCreateResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactInquiryCreateResponse>> Create(
        [FromBody] CreateContactInquiryDto request,
        CancellationToken cancellationToken)
    {
        var response = await inquiryApiCommandService.CreateContactInquiryAsync(request, cancellationToken);
        return Accepted(response);
    }
}
