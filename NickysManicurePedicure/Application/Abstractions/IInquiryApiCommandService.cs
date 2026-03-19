using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IInquiryApiCommandService
{
    Task<BookingCreateResponse> CreateBookingRequestAsync(CreateBookingRequestDto request, CancellationToken cancellationToken);

    Task<ContactInquiryCreateResponse> CreateContactInquiryAsync(CreateContactInquiryDto request, CancellationToken cancellationToken);
}
