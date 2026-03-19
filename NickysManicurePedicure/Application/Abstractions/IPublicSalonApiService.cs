using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IPublicSalonApiService
{
    Task<PagedResponse<SalonServiceResponse>> GetServicesAsync(ServiceCatalogQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<TestimonialResponse>> GetTestimonialsAsync(TestimonialQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<GalleryItemResponse>> GetGalleryItemsAsync(GalleryItemQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<FaqItemResponse>> GetFaqItemsAsync(FaqQueryParameters query, CancellationToken cancellationToken);

    Task<BusinessProfileResponse?> GetBusinessProfileAsync(CancellationToken cancellationToken);
}
