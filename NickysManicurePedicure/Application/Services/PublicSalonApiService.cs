using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Contracts.Common;
using NickysManicurePedicure.Contracts.Requests;
using NickysManicurePedicure.Contracts.Responses;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Extensions;

namespace NickysManicurePedicure.Application.Services;

public sealed class PublicSalonApiService(
    ApplicationDbContext dbContext,
    ILogger<PublicSalonApiService> logger) : IPublicSalonApiService
{
    public async Task<PagedResponse<SalonServiceResponse>> GetServicesAsync(
        ServiceCatalogQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var servicesQuery = dbContext.Services.AsNoTracking();

        if (query.FeaturedOnly == true)
        {
            servicesQuery = servicesQuery.Where(x => x.IsFeatured);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            servicesQuery = servicesQuery.Where(x =>
                x.Name.Contains(search) ||
                x.Description.Contains(search));
        }

        servicesQuery = (query.SortBy, query.SortDirection) switch
        {
            ("name", "desc") => servicesQuery.OrderByDescending(x => x.Name).ThenBy(x => x.DisplayOrder),
            ("name", _) => servicesQuery.OrderBy(x => x.Name).ThenBy(x => x.DisplayOrder),
            ("displayOrder", "desc") => servicesQuery.OrderByDescending(x => x.DisplayOrder).ThenBy(x => x.Name),
            _ => servicesQuery.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name)
        };

        logger.LogDebug("Retrieving paged services catalog. Page {Page}, PageSize {PageSize}.", query.Page, query.PageSize);

        return await servicesQuery
            .Select(x => new SalonServiceResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Duration = x.Duration,
                PriceFrom = x.PriceFrom,
                IsFeatured = x.IsFeatured,
                DisplayOrder = x.DisplayOrder
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<PagedResponse<TestimonialResponse>> GetTestimonialsAsync(
        TestimonialQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var testimonialsQuery = (query.SortBy, query.SortDirection) switch
        {
            ("clientName", "desc") => dbContext.Testimonials.AsNoTracking().OrderByDescending(x => x.ClientName).ThenBy(x => x.DisplayOrder),
            ("clientName", _) => dbContext.Testimonials.AsNoTracking().OrderBy(x => x.ClientName).ThenBy(x => x.DisplayOrder),
            ("displayOrder", "desc") => dbContext.Testimonials.AsNoTracking().OrderByDescending(x => x.DisplayOrder).ThenBy(x => x.ClientName),
            _ => dbContext.Testimonials.AsNoTracking().OrderBy(x => x.DisplayOrder).ThenBy(x => x.ClientName)
        };

        return await testimonialsQuery
            .Select(x => new TestimonialResponse
            {
                Id = x.Id,
                ClientName = x.ClientName,
                Highlight = x.Highlight,
                Review = x.Review,
                DisplayOrder = x.DisplayOrder
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<PagedResponse<GalleryItemResponse>> GetGalleryItemsAsync(
        GalleryItemQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var galleryQuery = dbContext.GalleryItems
            .AsNoTracking()
            .Where(x => x.IsPublished);

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.Trim();
            galleryQuery = galleryQuery.Where(x => x.Category == category);
        }

        if (query.FeaturedOnly == true)
        {
            galleryQuery = galleryQuery.Where(x => x.IsFeatured);
        }

        galleryQuery = (query.SortBy, query.SortDirection) switch
        {
            ("title", "desc") => galleryQuery.OrderByDescending(x => x.Title).ThenBy(x => x.DisplayOrder),
            ("title", _) => galleryQuery.OrderBy(x => x.Title).ThenBy(x => x.DisplayOrder),
            ("createdUtc", "desc") => galleryQuery.OrderByDescending(x => x.CreatedUtc).ThenBy(x => x.DisplayOrder),
            ("createdUtc", _) => galleryQuery.OrderBy(x => x.CreatedUtc).ThenBy(x => x.DisplayOrder),
            ("displayOrder", "desc") => galleryQuery.OrderByDescending(x => x.DisplayOrder).ThenBy(x => x.Title),
            _ => galleryQuery.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Title)
        };

        return await galleryQuery
            .Select(x => new GalleryItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Category = x.Category,
                ImageUrl = x.ImageUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                AltText = x.AltText,
                IsFeatured = x.IsFeatured,
                DisplayOrder = x.DisplayOrder,
                CreatedUtc = x.CreatedUtc
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<PagedResponse<FaqItemResponse>> GetFaqItemsAsync(
        FaqQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var faqQuery = dbContext.FaqItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            faqQuery = faqQuery.Where(x => x.Question.Contains(search) || x.Answer.Contains(search));
        }

        faqQuery = (query.SortBy, query.SortDirection) switch
        {
            ("question", "desc") => faqQuery.OrderByDescending(x => x.Question).ThenBy(x => x.DisplayOrder),
            ("question", _) => faqQuery.OrderBy(x => x.Question).ThenBy(x => x.DisplayOrder),
            ("displayOrder", "desc") => faqQuery.OrderByDescending(x => x.DisplayOrder).ThenBy(x => x.Question),
            _ => faqQuery.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Question)
        };

        return await faqQuery
            .Select(x => new FaqItemResponse
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                DisplayOrder = x.DisplayOrder
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<BusinessProfileResponse?> GetBusinessProfileAsync(CancellationToken cancellationToken)
    {
        var profile = await dbContext.BusinessProfiles
            .AsNoTracking()
            .Include(x => x.BusinessHours.OrderBy(hour => hour.DisplayOrder))
            .FirstOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            logger.LogWarning("Business profile was requested but no record exists.");
            return null;
        }

        return new BusinessProfileResponse
        {
            Id = profile.Id,
            Name = profile.Name,
            Tagline = profile.Tagline,
            Phone = profile.Phone,
            PhoneHref = profile.PhoneHref,
            Email = profile.Email,
            AddressLine1 = profile.AddressLine1,
            Suburb = profile.Suburb,
            City = profile.City,
            Region = profile.Region,
            PostalCode = profile.PostalCode,
            WhatsAppHref = profile.WhatsAppHref,
            InstagramHandle = profile.InstagramHandle,
            BookingPolicy = profile.BookingPolicy,
            AboutSummary = profile.AboutSummary,
            BusinessHours = profile.BusinessHours
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new BusinessHourResponse
                {
                    Id = x.Id,
                    DayOfWeek = x.DayOfWeek.ToString(),
                    IsClosed = x.IsClosed,
                    OpenTime = x.OpenTime,
                    CloseTime = x.CloseTime,
                    Notes = x.Notes,
                    DisplayOrder = x.DisplayOrder
                })
                .ToList()
        };
    }
}
