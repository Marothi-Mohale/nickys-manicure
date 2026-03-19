using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/bookings")]
[Route("api/booking-requests")]
[Produces("application/json")]
public sealed class BookingRequestsApiController(IBookingPublicCommandService bookingPublicCommandService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(BookingCreateResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingCreateResponse>> Create(
        [FromBody] CreateBookingRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await bookingPublicCommandService.CreateAsync(request, cancellationToken);
        return Created($"/api/bookings/{response.BookingId}", response);
    }
}
