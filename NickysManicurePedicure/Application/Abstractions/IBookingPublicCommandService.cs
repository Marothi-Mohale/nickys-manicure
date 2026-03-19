using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IBookingPublicCommandService
{
    Task<BookingCreateResponse> CreateAsync(CreateBookingRequestDto request, CancellationToken cancellationToken);
}
