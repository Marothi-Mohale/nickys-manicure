using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Application.Services;

public sealed class InquiryApiCommandService(
    ApplicationDbContext dbContext,
    ILogger<InquiryApiCommandService> logger) : IInquiryApiCommandService
{
    public async Task<BookingCreateResponse> CreateBookingRequestAsync(
        CreateBookingRequestDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var matchedService = await dbContext.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name == request.PreferredService.Trim(),
                cancellationToken);

        var bookingRequest = new BookingRequest
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            RequestedServiceName = request.PreferredService.Trim(),
            ServiceId = matchedService?.Id,
            PreferredDate = request.PreferredDate!.Value,
            PreferredTime = request.PreferredTime!.Value,
            Message = request.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(request.SourcePage) ? "Api" : request.SourcePage.Trim()
        };

        dbContext.BookingRequests.Add(bookingRequest);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created booking request {BookingRequestId} for {Email} from API source {SourcePage}.",
            bookingRequest.Id,
            bookingRequest.Email,
            bookingRequest.SourcePage);

        return new BookingCreateResponse
        {
            BookingId = bookingRequest.Id,
            Status = bookingRequest.Status.ToString(),
            Message = "Your booking request has been received. Our team will follow up shortly."
        };
    }

    public async Task<ContactInquiryCreateResponse> CreateContactInquiryAsync(
        CreateContactInquiryDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var contactInquiry = new ContactInquiry
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Subject = "General website inquiry",
            Message = request.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(request.SourcePage) ? "Api" : request.SourcePage.Trim()
        };

        dbContext.ContactInquiries.Add(contactInquiry);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created contact inquiry {ContactInquiryId} for {Email} from API source {SourcePage}.",
            contactInquiry.Id,
            contactInquiry.Email,
            contactInquiry.SourcePage);

        return new ContactInquiryCreateResponse
        {
            InquiryId = contactInquiry.Id,
            Status = contactInquiry.Status.ToString(),
            Message = "Your inquiry has been received. We will get back to you as soon as possible."
        };
    }
}
