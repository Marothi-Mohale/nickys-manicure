namespace NickysManicurePedicure.Services;

public sealed record SubmissionResult(bool Success, string Message, int? InquiryId = null);
