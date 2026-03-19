using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/booking-requests")]
[Produces("application/json")]
public sealed class BookingRequestsApiController(IInquiryApiCommandService inquiryApiCommandService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(BookingCreateResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingCreateResponse>> Create(
        [FromBody] CreateBookingRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await inquiryApiCommandService.CreateBookingRequestAsync(request, cancellationToken);
        return Accepted(response);
    }
}
