using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IBookingApiService
{
    Task<BookingCreateResponse> CreateAsync(CreateBookingRequestDto request, CancellationToken cancellationToken);

    Task<PagedResponse<BookingReadResponse>> GetBookingsAsync(BookingQueryParameters query, CancellationToken cancellationToken);

    Task<BookingReadResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<BookingReadResponse?> UpdateStatusAsync(int id, UpdateBookingStatusDto request, CancellationToken cancellationToken);
}
