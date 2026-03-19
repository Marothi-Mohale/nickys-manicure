using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Contracts.Requests;
using NickysManicurePedicure.Contracts.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/contact-inquiries")]
[Produces("application/json")]
public sealed class ContactInquiriesApiController(IInquiryApiCommandService inquiryApiCommandService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ContactInquiryAcceptedResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactInquiryAcceptedResponse>> Create(
        [FromBody] CreateContactInquiryDto request,
        CancellationToken cancellationToken)
    {
        var response = await inquiryApiCommandService.CreateContactInquiryAsync(request, cancellationToken);
        return Accepted(response);
    }
}
