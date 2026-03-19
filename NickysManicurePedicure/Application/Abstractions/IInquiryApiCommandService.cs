using NickysManicurePedicure.Contracts.Requests;
using NickysManicurePedicure.Contracts.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IInquiryApiCommandService
{
    Task<BookingRequestAcceptedResponse> CreateBookingRequestAsync(CreateBookingRequestDto request, CancellationToken cancellationToken);

    Task<ContactInquiryAcceptedResponse> CreateContactInquiryAsync(CreateContactInquiryDto request, CancellationToken cancellationToken);
}
