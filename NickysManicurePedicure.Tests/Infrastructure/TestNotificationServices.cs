using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Tests.Infrastructure;

public sealed class RecordingBookingNotificationService : IBookingNotificationService
{
    public int CreatedCallCount { get; private set; }
    public int StatusUpdatedCallCount { get; private set; }
    public BookingReadResponse? LastCreatedBooking { get; private set; }
    public BookingReadResponse? LastStatusUpdatedBooking { get; private set; }

    public Task OnBookingCreatedAsync(BookingReadResponse booking, CancellationToken cancellationToken)
    {
        CreatedCallCount++;
        LastCreatedBooking = booking;
        return Task.CompletedTask;
    }

    public Task OnBookingStatusUpdatedAsync(BookingReadResponse booking, CancellationToken cancellationToken)
    {
        StatusUpdatedCallCount++;
        LastStatusUpdatedBooking = booking;
        return Task.CompletedTask;
    }
}

public sealed class RecordingContactInquiryNotificationService : IContactInquiryNotificationService
{
    public int CreatedCallCount { get; private set; }
    public int StatusUpdatedCallCount { get; private set; }
    public ContactInquiryReadResponse? LastCreatedInquiry { get; private set; }
    public ContactInquiryReadResponse? LastStatusUpdatedInquiry { get; private set; }

    public Task OnInquiryCreatedAsync(ContactInquiryReadResponse inquiry, CancellationToken cancellationToken)
    {
        CreatedCallCount++;
        LastCreatedInquiry = inquiry;
        return Task.CompletedTask;
    }

    public Task OnInquiryStatusUpdatedAsync(ContactInquiryReadResponse inquiry, CancellationToken cancellationToken)
    {
        StatusUpdatedCallCount++;
        LastStatusUpdatedInquiry = inquiry;
        return Task.CompletedTask;
    }
}
