using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public interface IInquiryService
{
    Task<(bool Success, string Message)> CreateAsync(InquiryFormViewModel model, CancellationToken cancellationToken = default);
}
