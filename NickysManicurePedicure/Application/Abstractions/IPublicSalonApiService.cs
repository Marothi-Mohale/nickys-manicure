using NickysManicurePedicure.Contracts.Common;
using NickysManicurePedicure.Contracts.Requests;
using NickysManicurePedicure.Contracts.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IPublicSalonApiService
{
    Task<PagedResponse<SalonServiceResponse>> GetServicesAsync(ServiceCatalogQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<TestimonialResponse>> GetTestimonialsAsync(TestimonialQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<GalleryItemResponse>> GetGalleryItemsAsync(GalleryItemQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<FaqItemResponse>> GetFaqItemsAsync(FaqQueryParameters query, CancellationToken cancellationToken);

    Task<BusinessProfileResponse?> GetBusinessProfileAsync(CancellationToken cancellationToken);
}
