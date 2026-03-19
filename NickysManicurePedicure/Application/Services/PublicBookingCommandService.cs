using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Application.Services;

public sealed class PublicBookingCommandService(
    ApplicationDbContext dbContext,
    IBookingNotificationService bookingNotificationService,
    ILogger<PublicBookingCommandService> logger) : IBookingPublicCommandService
{
    public async Task<BookingCreateResponse> CreateAsync(
        CreateBookingRequestDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var matchedService = await ResolvePreferredServiceAsync(request, cancellationToken);

        var booking = new BookingRequest
        {
            FullName = request.FullName.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Email = request.Email.Trim(),
            ServiceId = matchedService?.Id,
            RequestedServiceName = matchedService?.Name ?? request.PreferredServiceName!.Trim(),
            PreferredDate = request.PreferredDate!.Value,
            PreferredTime = request.PreferredTime!.Value,
            Message = request.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(request.SourcePage) ? "Api" : request.SourcePage.Trim()
        };

        dbContext.BookingRequests.Add(booking);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = MapBookingReadResponse(booking);

        logger.LogInformation(
            "Created booking {BookingId} for {Email} on {PreferredDate} at {PreferredTime}.",
            booking.Id,
            booking.Email,
            booking.PreferredDate,
            booking.PreferredTime);

        await bookingNotificationService.OnBookingCreatedAsync(response, cancellationToken);

        return new BookingCreateResponse
        {
            BookingId = booking.Id,
            Status = booking.Status.ToString(),
            Message = "Your booking request has been received. We will confirm availability and follow up shortly.",
            DetailUrl = $"/api/bookings/{booking.Id}"
        };
    }

    private async Task<Service?> ResolvePreferredServiceAsync(CreateBookingRequestDto request, CancellationToken cancellationToken)
    {
        if (request.PreferredServiceId is not null)
        {
            var matchedService = await dbContext.Services
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published)
                .FirstOrDefaultAsync(x => x.Id == request.PreferredServiceId.Value, cancellationToken);

            return matchedService ?? throw new BadRequestException(
                "The selected service could not be found.",
                "invalid_service_reference");
        }

        if (string.IsNullOrWhiteSpace(request.PreferredServiceName))
        {
            return null;
        }

        var trimmedName = request.PreferredServiceName.Trim();
        var namedService = await dbContext.Services
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published)
            .FirstOrDefaultAsync(x => x.Name == trimmedName, cancellationToken);

        return namedService ?? throw new BadRequestException(
            "The selected service could not be found.",
            "invalid_service_reference");
    }

    private static BookingReadResponse MapBookingReadResponse(BookingRequest booking) => new()
    {
        Id = booking.Id,
        PreferredServiceId = booking.ServiceId,
        PreferredServiceName = booking.RequestedServiceName,
        Status = booking.Status.ToString(),
        FullName = booking.FullName,
        Email = booking.Email,
        PhoneNumber = booking.PhoneNumber,
        PreferredDate = booking.PreferredDate,
        PreferredTime = booking.PreferredTime,
        Message = booking.Message,
        SourcePage = booking.SourcePage,
        AdminNotes = booking.AdminNotes,
        CreatedAtUtc = booking.CreatedAtUtc,
        UpdatedAtUtc = booking.UpdatedAtUtc
    };
}
