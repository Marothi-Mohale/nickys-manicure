using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IBookingAdminService
{
    Task<PagedResponse<BookingReadResponse>> GetBookingsAsync(BookingQueryParameters query, CancellationToken cancellationToken);

    Task<BookingReadResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<BookingReadResponse?> UpdateStatusAsync(int id, UpdateBookingStatusDto request, CancellationToken cancellationToken);
}
