using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace NickysManicurePedicure.Application.Services;

public sealed class BookingAdminService(
    ApplicationDbContext dbContext,
    IBookingNotificationService bookingNotificationService,
    ILogger<BookingAdminService> logger) : IBookingAdminService
{
    public async Task<PagedResponse<BookingReadResponse>> GetBookingsAsync(
        BookingQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var bookingsQuery = dbContext.BookingRequests.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            bookingsQuery = bookingsQuery.Where(x =>
                x.FullName.Contains(search) ||
                x.Email.Contains(search) ||
                x.PhoneNumber.Contains(search) ||
                x.RequestedServiceName.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.Status)
            && Enum.TryParse<Models.Entities.BookingRequestStatus>(query.Status, ignoreCase: true, out var status))
        {
            bookingsQuery = bookingsQuery.Where(x => x.Status == status);
        }

        if (query.FromDate is not null)
        {
            bookingsQuery = bookingsQuery.Where(x => x.PreferredDate >= query.FromDate);
        }

        if (query.ToDate is not null)
        {
            bookingsQuery = bookingsQuery.Where(x => x.PreferredDate <= query.ToDate);
        }

        bookingsQuery = (query.SortBy, query.SortDirection) switch
        {
            ("preferredDate", "asc") => bookingsQuery.OrderBy(x => x.PreferredDate).ThenBy(x => x.PreferredTime).ThenBy(x => x.Id),
            ("preferredDate", _) => bookingsQuery.OrderByDescending(x => x.PreferredDate).ThenByDescending(x => x.PreferredTime).ThenByDescending(x => x.Id),
            ("status", "asc") => bookingsQuery.OrderBy(x => x.Status).ThenByDescending(x => x.CreatedAtUtc),
            ("status", _) => bookingsQuery.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreatedAtUtc),
            ("createdAtUtc", "asc") => bookingsQuery.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.Id),
            _ => bookingsQuery.OrderByDescending(x => x.CreatedAtUtc).ThenByDescending(x => x.Id)
        };

        return await bookingsQuery
            .Select(x => new BookingReadResponse
            {
                Id = x.Id,
                PreferredServiceId = x.ServiceId,
                PreferredServiceName = x.RequestedServiceName,
                Status = x.Status.ToString(),
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                PreferredDate = x.PreferredDate,
                PreferredTime = x.PreferredTime,
                Message = x.Message,
                SourcePage = x.SourcePage,
                AdminNotes = x.AdminNotes,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<BookingReadResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.BookingRequests
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new BookingReadResponse
            {
                Id = x.Id,
                PreferredServiceId = x.ServiceId,
                PreferredServiceName = x.RequestedServiceName,
                Status = x.Status.ToString(),
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                PreferredDate = x.PreferredDate,
                PreferredTime = x.PreferredTime,
                Message = x.Message,
                SourcePage = x.SourcePage,
                AdminNotes = x.AdminNotes,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<BookingReadResponse?> UpdateStatusAsync(
        int id,
        UpdateBookingStatusDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var booking = await dbContext.BookingRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (booking is null)
        {
            return null;
        }

        if (!Enum.TryParse<Models.Entities.BookingRequestStatus>(request.Status, ignoreCase: true, out var parsedStatus))
        {
            throw new BadRequestException("The requested booking status is invalid.", "invalid_booking_status");
        }

        booking.Status = parsedStatus;
        booking.AdminNotes = request.AdminNotes is null
            ? booking.AdminNotes
            : string.IsNullOrWhiteSpace(request.AdminNotes) ? null : request.AdminNotes.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = MapBookingReadResponse(booking);

        logger.LogInformation(
            "Updated booking {BookingId} to status {Status}.",
            booking.Id,
            booking.Status);

        await bookingNotificationService.OnBookingStatusUpdatedAsync(response, cancellationToken);

        return response;
    }

    private static BookingReadResponse MapBookingReadResponse(Models.Entities.BookingRequest booking) => new()
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
