using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers.Admin;

[Route("api/admin/bookings")]
[Route("api/bookings")]
[Route("api/booking-requests")]
public sealed class AdminBookingsApiController(IBookingAdminService bookingAdminService) : AdminControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<BookingReadResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<BookingReadResponse>>> GetList(
        [FromQuery] BookingQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await bookingAdminService.GetBookingsAsync(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookingReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingReadResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var booking = await bookingAdminService.GetByIdAsync(id, cancellationToken);
        return Ok(booking ?? throw new NotFoundException("Booking", id));
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(BookingReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingReadResponse>> UpdateStatus(
        int id,
        [FromBody] UpdateBookingStatusDto request,
        CancellationToken cancellationToken)
    {
        var booking = await bookingAdminService.UpdateStatusAsync(id, request, cancellationToken);
        return Ok(booking ?? throw new NotFoundException("Booking", id));
    }
}
