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
    public async Task<BookingRequestAcceptedResponse> CreateBookingRequestAsync(
        CreateBookingRequestDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inquiry = new Inquiry
        {
            InquiryType = InquiryType.Booking,
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            PreferredService = request.PreferredService.Trim(),
            PreferredDate = request.PreferredDate,
            PreferredTime = request.PreferredTime,
            Message = request.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(request.SourcePage) ? "Api" : request.SourcePage.Trim()
        };

        dbContext.Inquiries.Add(inquiry);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created booking inquiry {InquiryId} for {Email} from API source {SourcePage}.",
            inquiry.Id,
            inquiry.Email,
            inquiry.SourcePage);

        return new BookingRequestAcceptedResponse
        {
            InquiryId = inquiry.Id,
            Message = "Your booking request has been received. Our team will follow up shortly."
        };
    }

    public async Task<ContactInquiryAcceptedResponse> CreateContactInquiryAsync(
        CreateContactInquiryDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inquiry = new Inquiry
        {
            InquiryType = InquiryType.General,
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Message = request.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(request.SourcePage) ? "Api" : request.SourcePage.Trim()
        };

        dbContext.Inquiries.Add(inquiry);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created contact inquiry {InquiryId} for {Email} from API source {SourcePage}.",
            inquiry.Id,
            inquiry.Email,
            inquiry.SourcePage);

        return new ContactInquiryAcceptedResponse
        {
            InquiryId = inquiry.Id,
            Message = "Your inquiry has been received. We will get back to you as soon as possible."
        };
    }
}
