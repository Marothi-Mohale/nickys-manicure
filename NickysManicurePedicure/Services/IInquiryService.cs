using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public interface IInquiryService
{
    Task<SubmissionResult> CreateAsync(InquiryFormViewModel model, CancellationToken cancellationToken = default);
}
