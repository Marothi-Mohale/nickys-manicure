using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IContactInquiryNotificationService
{
    Task OnInquiryCreatedAsync(ContactInquiryReadResponse inquiry, CancellationToken cancellationToken);

    Task OnInquiryStatusUpdatedAsync(ContactInquiryReadResponse inquiry, CancellationToken cancellationToken);
}
