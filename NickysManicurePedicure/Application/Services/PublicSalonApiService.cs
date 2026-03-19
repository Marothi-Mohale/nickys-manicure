using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Extensions;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Application.Services;

public sealed class PublicSalonApiService(
    ApplicationDbContext dbContext,
    ILogger<PublicSalonApiService> logger) : IPublicSalonApiService
{
    public async Task<PagedResponse<ServiceListItemResponse>> GetServicesAsync(
        ServiceCatalogQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var servicesQuery = dbContext.Services
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published);

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

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.Trim().ToLowerInvariant();
            servicesQuery = servicesQuery.Where(x => x.ServiceCategory != null && x.ServiceCategory.Slug == category);
        }

        servicesQuery = (query.SortBy, query.SortDirection) switch
        {
            ("name", "desc") => servicesQuery.OrderByDescending(x => x.Name).ThenBy(x => x.DisplayOrder),
            ("name", _) => servicesQuery.OrderBy(x => x.Name).ThenBy(x => x.DisplayOrder),
            ("price", "desc") => servicesQuery.OrderByDescending(x => x.PriceFromAmount.HasValue)
                .ThenByDescending(x => x.PriceFromAmount)
                .ThenBy(x => x.DisplayOrder),
            ("price", _) => servicesQuery.OrderByDescending(x => x.PriceFromAmount.HasValue)
                .ThenBy(x => x.PriceFromAmount)
                .ThenBy(x => x.DisplayOrder),
            ("displayOrder", "desc") => servicesQuery.OrderByDescending(x => x.DisplayOrder).ThenBy(x => x.Name),
            _ => servicesQuery.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name)
        };

        logger.LogDebug("Retrieving paged services catalog. Page {Page}, PageSize {PageSize}.", query.Page, query.PageSize);

        return await servicesQuery
            .Select(x => new ServiceListItemResponse
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                DurationLabel = x.DurationLabel,
                PriceFromLabel = x.PriceFromLabel,
                PriceFromAmount = x.PriceFromAmount,
                IsFeatured = x.IsFeatured,
                DisplayOrder = x.DisplayOrder,
                Category = new ServiceCategorySummaryResponse
                {
                    Id = x.ServiceCategoryId,
                    Name = x.ServiceCategory!.Name,
                    Slug = x.ServiceCategory.Slug,
                    Description = x.ServiceCategory.Description
                }
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<ServiceDetailResponse?> GetServiceByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Services
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published)
            .Where(x => x.Id == id)
            .Select(x => new ServiceDetailResponse
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                DurationLabel = x.DurationLabel,
                PriceFromLabel = x.PriceFromLabel,
                PriceFromAmount = x.PriceFromAmount,
                IsFeatured = x.IsFeatured,
                DisplayOrder = x.DisplayOrder,
                Category = new ServiceCategorySummaryResponse
                {
                    Id = x.ServiceCategoryId,
                    Name = x.ServiceCategory!.Name,
                    Slug = x.ServiceCategory.Slug,
                    Description = x.ServiceCategory.Description
                }
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ServiceCategoryListItemResponse>> GetServiceCategoriesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.ServiceCategories
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published)
            .OrderBy(x => x.DisplayOrder)
            .Select(x => new ServiceCategoryListItemResponse
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                ServiceCount = x.Services.Count(service => service.Status == ContentStatus.Published)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResponse<TestimonialListItemResponse>> GetTestimonialsAsync(
        TestimonialQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var testimonialsBaseQuery = dbContext.Testimonials
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published && x.IsApproved);

        var testimonialsQuery = (query.SortBy, query.SortDirection) switch
        {
            ("clientName", "desc") => testimonialsBaseQuery.OrderByDescending(x => x.ClientName).ThenBy(x => x.DisplayOrder),
            ("clientName", _) => testimonialsBaseQuery.OrderBy(x => x.ClientName).ThenBy(x => x.DisplayOrder),
            ("displayOrder", "desc") => testimonialsBaseQuery.OrderByDescending(x => x.DisplayOrder).ThenBy(x => x.ClientName),
            _ => testimonialsBaseQuery.OrderBy(x => x.DisplayOrder).ThenBy(x => x.ClientName)
        };

        return await testimonialsQuery
            .Select(x => new TestimonialListItemResponse
            {
                Id = x.Id,
                ClientName = x.ClientName,
                Quote = x.Quote,
                Rating = x.Rating,
                IsFeatured = x.IsFeatured,
                DisplayOrder = x.DisplayOrder
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<PagedResponse<GalleryListItemResponse>> GetGalleryItemsAsync(
        GalleryItemQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var galleryQuery = dbContext.GalleryItems
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published);

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.Trim().ToLowerInvariant();
            galleryQuery = galleryQuery.Where(x => x.Category != null && x.Category.ToLower() == category);
        }

        if (query.FeaturedOnly == true)
        {
            galleryQuery = galleryQuery.Where(x => x.IsFeatured);
        }

        galleryQuery = (query.SortBy, query.SortDirection) switch
        {
            ("title", "desc") => galleryQuery.OrderByDescending(x => x.Title).ThenBy(x => x.DisplayOrder),
            ("title", _) => galleryQuery.OrderBy(x => x.Title).ThenBy(x => x.DisplayOrder),
            ("createdUtc", "desc") => galleryQuery.OrderByDescending(x => x.CreatedAtUtc).ThenBy(x => x.DisplayOrder),
            ("createdUtc", _) => galleryQuery.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.DisplayOrder),
            ("displayOrder", "desc") => galleryQuery.OrderByDescending(x => x.DisplayOrder).ThenBy(x => x.Title),
            _ => galleryQuery.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Title)
        };

        return await galleryQuery
            .Select(x => new GalleryListItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Category = x.Category,
                ImageUrl = x.ImageUrl,
                AltText = x.AltText,
                IsFeatured = x.IsFeatured,
                DisplayOrder = x.DisplayOrder
            })
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<PagedResponse<FaqListItemResponse>> GetFaqItemsAsync(
        FaqQueryParameters query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var faqQuery = dbContext.FaqItems
            .AsNoTracking()
            .Where(x => x.Status == ContentStatus.Published && x.IsActive);

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
            .Select(x => new FaqListItemResponse
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
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            logger.LogWarning("Business profile was requested but no record exists.");
            return null;
        }

        return new BusinessProfileResponse
        {
            Id = profile.Id,
            BusinessName = profile.Name,
            Tagline = profile.Tagline,
            Description = profile.Description,
            PhoneNumber = profile.Phone,
            Email = profile.Email,
            AddressLine1 = profile.AddressLine1,
            City = profile.City,
            Region = profile.Region,
            PostalCode = profile.PostalCode,
            InstagramHandle = profile.InstagramHandle,
            YearsOfExperience = profile.YearsOfExperience,
            HeroHeadline = profile.HeroHeadline,
            HeroSubheadline = profile.HeroSubheadline
        };
    }

    public async Task<IReadOnlyCollection<BusinessHourResponse>> GetBusinessHoursAsync(CancellationToken cancellationToken)
    {
        var profile = await dbContext.BusinessProfiles
            .AsNoTracking()
            .Include(x => x.BusinessHours.OrderBy(hour => hour.DisplayOrder))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            logger.LogWarning("Business hours were requested but no business profile exists.");
            return [];
        }

        return profile.BusinessHours
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
            .ToList();
    }
}
