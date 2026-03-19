using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IPublicSalonApiService
{
    Task<PagedResponse<ServiceListItemResponse>> GetServicesAsync(ServiceCatalogQueryParameters query, CancellationToken cancellationToken);

    Task<ServiceDetailResponse?> GetServiceByIdAsync(int id, CancellationToken cancellationToken);

    Task<PagedResponse<TestimonialListItemResponse>> GetTestimonialsAsync(TestimonialQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<GalleryListItemResponse>> GetGalleryItemsAsync(GalleryItemQueryParameters query, CancellationToken cancellationToken);

    Task<PagedResponse<FaqListItemResponse>> GetFaqItemsAsync(FaqQueryParameters query, CancellationToken cancellationToken);

    Task<BusinessProfileResponse?> GetBusinessProfileAsync(CancellationToken cancellationToken);
}
