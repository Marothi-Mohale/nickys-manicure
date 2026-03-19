using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Extensions;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Application.Services;

public sealed class ContactInquiryAdminService(
    ApplicationDbContext dbContext,
    IContactInquiryNotificationService notificationService,
    ILogger<ContactInquiryAdminService> logger) : IContactInquiryAdminService
{
    public async Task<PagedResponse<ContactInquiryReadResponse>> GetInquiriesAsync(
        ContactInquiryQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var inquiries = dbContext.ContactInquiries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            inquiries = inquiries.Where(x =>
                x.FullName.Contains(search) ||
                x.Email.Contains(search) ||
                x.PhoneNumber.Contains(search) ||
                (x.Subject != null && x.Subject.Contains(search)) ||
                x.Message.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.Status)
            && Enum.TryParse<ContactInquiryStatus>(query.Status, ignoreCase: true, out var status))
        {
            inquiries = inquiries.Where(x => x.Status == status);
        }

        inquiries = (query.SortBy, query.SortDirection) switch
        {
            ("status", "asc") => inquiries.OrderBy(x => x.Status).ThenByDescending(x => x.CreatedAtUtc),
            ("status", _) => inquiries.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreatedAtUtc),
            ("fullName", "asc") => inquiries.OrderBy(x => x.FullName).ThenByDescending(x => x.CreatedAtUtc),
            ("fullName", _) => inquiries.OrderByDescending(x => x.FullName).ThenByDescending(x => x.CreatedAtUtc),
            ("createdAtUtc", "asc") => inquiries.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.Id),
            _ => inquiries.OrderByDescending(x => x.CreatedAtUtc).ThenByDescending(x => x.Id)
        };

        return await inquiries
            .Select(x => new ContactInquiryReadResponse
            {
                Id = x.Id,
                Status = x.Status.ToString(),
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Subject = x.Subject,
                Message = x.Message,
                SourcePage = x.SourcePage,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<ContactInquiryReadResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.ContactInquiries
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ContactInquiryReadResponse
            {
                Id = x.Id,
                Status = x.Status.ToString(),
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Subject = x.Subject,
                Message = x.Message,
                SourcePage = x.SourcePage,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ContactInquiryReadResponse?> UpdateStatusAsync(
        int id,
        UpdateContactInquiryStatusDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inquiry = await dbContext.ContactInquiries.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (inquiry is null)
        {
            return null;
        }

        if (!Enum.TryParse<ContactInquiryStatus>(request.Status, ignoreCase: true, out var parsedStatus))
        {
            throw new BadRequestException("The requested inquiry status is invalid.", "invalid_inquiry_status");
        }

        inquiry.Status = parsedStatus;
        inquiry.AdminNotes = request.AdminNotes is null
            ? inquiry.AdminNotes
            : string.IsNullOrWhiteSpace(request.AdminNotes) ? null : request.AdminNotes.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = MapReadResponse(inquiry);

        logger.LogInformation(
            "Updated contact inquiry {InquiryId} to status {Status}.",
            inquiry.Id,
            inquiry.Status);

        await notificationService.OnInquiryStatusUpdatedAsync(response, cancellationToken);

        return response;
    }

    private static ContactInquiryReadResponse MapReadResponse(ContactInquiry inquiry) => new()
    {
        Id = inquiry.Id,
        Status = inquiry.Status.ToString(),
        FullName = inquiry.FullName,
        Email = inquiry.Email,
        PhoneNumber = inquiry.PhoneNumber,
        Subject = inquiry.Subject,
        Message = inquiry.Message,
        SourcePage = inquiry.SourcePage,
        CreatedAtUtc = inquiry.CreatedAtUtc,
        UpdatedAtUtc = inquiry.UpdatedAtUtc
    };
}
