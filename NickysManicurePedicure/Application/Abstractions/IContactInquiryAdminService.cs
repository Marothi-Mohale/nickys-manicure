using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IContactInquiryAdminService
{
    Task<PagedResponse<ContactInquiryReadResponse>> GetInquiriesAsync(ContactInquiryQueryParameters query, CancellationToken cancellationToken);

    Task<ContactInquiryReadResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<ContactInquiryReadResponse?> UpdateStatusAsync(int id, UpdateContactInquiryStatusDto request, CancellationToken cancellationToken);
}
