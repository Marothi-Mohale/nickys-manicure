using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IContactInquiryPublicCommandService
{
    Task<ContactInquiryCreateResponse> CreateAsync(CreateContactInquiryDto request, CancellationToken cancellationToken);
}
