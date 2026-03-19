using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/bookings")]
[Route("api/booking-requests")]
[Produces("application/json")]
public sealed class BookingsApiController(IBookingApiService bookingApiService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(BookingCreateResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingCreateResponse>> Create(
        [FromBody] CreateBookingRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await bookingApiService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.BookingId }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<BookingReadResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<BookingReadResponse>>> GetList(
        [FromQuery] BookingQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await bookingApiService.GetBookingsAsync(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookingReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingReadResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var booking = await bookingApiService.GetByIdAsync(id, cancellationToken);
        return booking is null ? NotFound() : Ok(booking);
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
        var booking = await bookingApiService.UpdateStatusAsync(id, request, cancellationToken);
        return booking is null ? NotFound() : Ok(booking);
    }
}
