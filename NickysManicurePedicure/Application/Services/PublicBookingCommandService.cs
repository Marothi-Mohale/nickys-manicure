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

        var serviceSelection = await ResolvePreferredServiceAsync(request, cancellationToken);

        var booking = new BookingRequest
        {
            FullName = request.FullName.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Email = request.Email.Trim(),
            ServiceId = serviceSelection.Service?.Id,
            RequestedServiceName = serviceSelection.RequestedServiceName,
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

    private async Task<ResolvedServiceSelection> ResolvePreferredServiceAsync(
        CreateBookingRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request.PreferredServiceId is not null)
        {
            var matchedService = await dbContext.Services
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published)
                .FirstOrDefaultAsync(x => x.Id == request.PreferredServiceId.Value, cancellationToken);

            if (matchedService is null)
            {
                throw new BadRequestException(
                    "The selected service could not be found.",
                    "invalid_service_reference");
            }

            if (!string.IsNullOrWhiteSpace(request.PreferredServiceName)
                && !string.Equals(matchedService.Name, request.PreferredServiceName.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException(
                    "The preferred service name does not match the selected service id.",
                    "invalid_service_reference");
            }

            return new ResolvedServiceSelection(matchedService, matchedService.Name);
        }

        if (string.IsNullOrWhiteSpace(request.PreferredServiceName))
        {
            throw new BadRequestException(
                "Either preferred service id or preferred service name is required.",
                "invalid_service_reference");
        }

        var trimmedName = request.PreferredServiceName.Trim();
        var matchedByName = await dbContext.Services
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published)
            .FirstOrDefaultAsync(x => x.Name.ToLower() == trimmedName.ToLower(), cancellationToken);

        return matchedByName is null
            ? new ResolvedServiceSelection(null, trimmedName)
            : new ResolvedServiceSelection(matchedByName, matchedByName.Name);
    }

    private sealed record ResolvedServiceSelection(Service? Service, string RequestedServiceName);

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
