using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Services;

public sealed class NullContactInquiryNotificationService : IContactInquiryNotificationService
{
    public Task OnInquiryCreatedAsync(ContactInquiryReadResponse inquiry, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnInquiryStatusUpdatedAsync(ContactInquiryReadResponse inquiry, CancellationToken cancellationToken) => Task.CompletedTask;
}
