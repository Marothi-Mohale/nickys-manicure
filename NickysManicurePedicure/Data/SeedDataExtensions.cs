using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.Data;

public static class SeedDataExtensions
{
    public static IReadOnlyList<SalonService> ServicesSeed =>
    [
        new() { Id = 1, Name = "Luxury Signature Manicure", Description = "Precision cuticle care, nail shaping, hydration ritual, and a high-gloss polish finish.", Duration = "60 min", PriceFrom = "From R280", IsFeatured = true, DisplayOrder = 1 },
        new() { Id = 2, Name = "Luxury Signature Pedicure", Description = "An indulgent pedicure with soak, exfoliation, expert nail care, and beautifully finished toes.", Duration = "75 min", PriceFrom = "From R340", IsFeatured = true, DisplayOrder = 2 },
        new() { Id = 3, Name = "Gel Polish Refresh", Description = "Long-wear gel application for women who want a polished look that lasts through a busy week.", Duration = "50 min", PriceFrom = "From R250", IsFeatured = true, DisplayOrder = 3 },
        new() { Id = 4, Name = "Nail Art Detail Work", Description = "Refined custom accents and elevated design details tailored to your occasion and personal style.", Duration = "30 - 45 min add-on", PriceFrom = "From R120", IsFeatured = false, DisplayOrder = 4 },
        new() { Id = 5, Name = "Bridal or Occasion Nail Prep", Description = "Curated manicure and pedicure preparation for weddings, events, and special photo-ready moments.", Duration = "By consultation", PriceFrom = "Custom quote", IsFeatured = false, DisplayOrder = 5 },
        new() { Id = 6, Name = "Restorative Nail Care Session", Description = "A gentle treatment-focused appointment for tired nails needing nourishment, reshaping, and recovery.", Duration = "45 min", PriceFrom = "From R220", IsFeatured = false, DisplayOrder = 6 }
    ];

    public static IReadOnlyList<Testimonial> TestimonialsSeed =>
    [
        new() { Id = 1, ClientName = "Lerato M.", Highlight = "Elegant every time", Review = "Nicky is warm, professional, and incredibly precise. My nails always look elegant and last beautifully.", DisplayOrder = 1 },
        new() { Id = 2, ClientName = "Ayanda P.", Highlight = "Luxury and consistency", Review = "The experience feels premium from the moment you arrive. Clean, calm, and consistently excellent results.", DisplayOrder = 2 },
        new() { Id = 3, ClientName = "Zanele R.", Highlight = "Trusted for special occasions", Review = "She prepared my nails for an event and I felt polished, feminine, and completely looked after.", DisplayOrder = 3 }
    ];

    public static IReadOnlyList<FaqItem> FaqSeed =>
    [
        new() { Id = 1, Question = "Do I need to book in advance?", Answer = "Advance booking is recommended, especially for Fridays, Saturdays, and special occasion appointments.", DisplayOrder = 1 },
        new() { Id = 2, Question = "Can I request a preferred date or time?", Answer = "Yes. Share your ideal appointment time in the booking form and we will confirm availability as quickly as possible.", DisplayOrder = 2 },
        new() { Id = 3, Question = "Do you offer custom nail art?", Answer = "Yes. Subtle luxury finishes and bespoke nail art details can be discussed when you submit your request.", DisplayOrder = 3 },
        new() { Id = 4, Question = "Where is the salon located?", Answer = "We are based at 72 Main Road, Mowbray, Cape Town, making visits easy for clients across the city.", DisplayOrder = 4 }
    ];

    public static IReadOnlyList<GalleryItem> GallerySeed =>
    [
        new() { Id = 1, Title = "Signature Nude Gloss Set", Description = "Luxury nude manicure styling with refined finish work.", Category = "Manicure", ImageUrl = "/images/gallery/signature-nude-gloss.jpg", ThumbnailUrl = "/images/gallery/thumbs/signature-nude-gloss.jpg", AltText = "Elegant nude manicure with glossy finish", IsFeatured = true, IsPublished = true, DisplayOrder = 1, CreatedUtc = DateTime.UtcNow.AddDays(-30) },
        new() { Id = 2, Title = "Soft Bridal Detail", Description = "A bridal-ready set with delicate detail accents and clean structure.", Category = "Bridal", ImageUrl = "/images/gallery/soft-bridal-detail.jpg", ThumbnailUrl = "/images/gallery/thumbs/soft-bridal-detail.jpg", AltText = "Bridal nail styling with soft luxury detail", IsFeatured = true, IsPublished = true, DisplayOrder = 2, CreatedUtc = DateTime.UtcNow.AddDays(-25) },
        new() { Id = 3, Title = "Polished Pedicure Finish", Description = "High-end pedicure presentation for sandal-ready confidence.", Category = "Pedicure", ImageUrl = "/images/gallery/polished-pedicure-finish.jpg", ThumbnailUrl = "/images/gallery/thumbs/polished-pedicure-finish.jpg", AltText = "Luxury pedicure finish in neutral tones", IsFeatured = false, IsPublished = true, DisplayOrder = 3, CreatedUtc = DateTime.UtcNow.AddDays(-20) }
    ];

    public static async Task EnsureDatabaseReadyAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitialization");
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var databaseOptions = scope.ServiceProvider
            .GetRequiredService<IOptions<DatabaseOptions>>()
            .Value;
        var businessProfileOptions = scope.ServiceProvider
            .GetRequiredService<IOptions<BusinessProfileOptions>>()
            .Value;

        try
        {
            await InitializeSchemaAsync(dbContext, databaseOptions, logger);

            if (!databaseOptions.SeedOnStartup)
            {
                logger.LogInformation("Database seeding is disabled.");
                return;
            }

            await SeedMissingAsync(dbContext.Services, ServicesSeed, item => item.Id, "services", logger);
            await SeedMissingAsync(dbContext.Testimonials, TestimonialsSeed, item => item.Id, "testimonials", logger);
            await SeedMissingAsync(dbContext.FaqItems, FaqSeed, item => item.Id, "faq items", logger);
            await SeedMissingAsync(dbContext.GalleryItems, GallerySeed, item => item.Id, "gallery items", logger);
            await SeedBusinessProfileAsync(dbContext, businessProfileOptions, logger);

            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Database seed changes were saved successfully.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to initialize the database.");
            throw;
        }
    }

    private static async Task InitializeSchemaAsync(
        ApplicationDbContext dbContext,
        DatabaseOptions databaseOptions,
        ILogger logger)
    {
        var hasMigrations = dbContext.Database.GetMigrations().Any();

        if (databaseOptions.ApplyMigrationsOnStartup && hasMigrations)
        {
            logger.LogInformation("Applying database migrations on startup.");
            await dbContext.Database.MigrateAsync();
            return;
        }

        if (databaseOptions.ApplyMigrationsOnStartup && !hasMigrations)
        {
            logger.LogWarning("ApplyMigrationsOnStartup is enabled, but no migrations were found. Falling back to EnsureCreated.");
        }

        logger.LogInformation("Ensuring database schema is created without migrations.");
        await dbContext.Database.EnsureCreatedAsync();
    }

    private static async Task SeedMissingAsync<TEntity, TKey>(
        DbSet<TEntity> dbSet,
        IReadOnlyCollection<TEntity> seedItems,
        Func<TEntity, TKey> keySelector,
        string entityName,
        ILogger logger)
        where TEntity : class
        where TKey : notnull
    {
        var existingEntities = await dbSet
            .AsNoTracking()
            .ToListAsync();
        var existingKeySet = existingEntities
            .Select(keySelector)
            .ToHashSet();
        var missingItems = seedItems
            .Where(item => !existingKeySet.Contains(keySelector(item)))
            .ToList();

        if (missingItems.Count == 0)
        {
            logger.LogInformation("No missing seed data detected for {EntityName}.", entityName);
            return;
        }

        await dbSet.AddRangeAsync(missingItems);
        logger.LogInformation("Queued {Count} missing {EntityName} seed records.", missingItems.Count, entityName);
    }

    private static async Task SeedBusinessProfileAsync(
        ApplicationDbContext dbContext,
        BusinessProfileOptions options,
        ILogger logger)
    {
        var profile = await dbContext.BusinessProfiles
            .Include(x => x.BusinessHours)
            .FirstOrDefaultAsync(x => x.Id == 1);

        if (profile is null)
        {
            profile = new BusinessProfile
            {
                Id = 1,
                Name = options.Name,
                Tagline = options.Tagline,
                Phone = options.Phone,
                PhoneHref = options.PhoneHref,
                Email = options.Email,
                AddressLine1 = options.AddressLine1,
                Suburb = options.Suburb,
                City = options.City,
                Region = options.Region,
                PostalCode = options.PostalCode,
                WhatsAppHref = options.WhatsAppHref,
                InstagramHandle = options.InstagramHandle,
                BookingPolicy = "Booking requests are confirmed manually to preserve scheduling quality and a premium client experience.",
                AboutSummary = "Nicky's Manicure & Pedicure delivers luxury manicure and pedicure services with thoughtful care, elegant finishes, and a calm, premium experience in Cape Town."
            };

            dbContext.BusinessProfiles.Add(profile);
            logger.LogInformation("Queued initial business profile seed record.");
        }
        else
        {
            profile.Name = options.Name;
            profile.Tagline = options.Tagline;
            profile.Phone = options.Phone;
            profile.PhoneHref = options.PhoneHref;
            profile.Email = options.Email;
            profile.AddressLine1 = options.AddressLine1;
            profile.Suburb = options.Suburb;
            profile.City = options.City;
            profile.Region = options.Region;
            profile.PostalCode = options.PostalCode;
            profile.WhatsAppHref = options.WhatsAppHref;
            profile.InstagramHandle = options.InstagramHandle;
            profile.UpdatedUtc = DateTime.UtcNow;
        }

        var seededHours = BuildBusinessHourSeed();
        var existingDays = profile.BusinessHours
            .Select(x => x.DayOfWeek)
            .ToHashSet();

        foreach (var businessHour in seededHours.Where(hour => !existingDays.Contains(hour.DayOfWeek)))
        {
            profile.BusinessHours.Add(businessHour);
        }
    }

    private static IReadOnlyList<BusinessHour> BuildBusinessHourSeed() =>
    [
        new() { Id = 1, BusinessProfileId = 1, DayOfWeek = DayOfWeek.Monday, IsClosed = false, OpenTime = new TimeOnly(9, 0), CloseTime = new TimeOnly(18, 0), DisplayOrder = 1 },
        new() { Id = 2, BusinessProfileId = 1, DayOfWeek = DayOfWeek.Tuesday, IsClosed = false, OpenTime = new TimeOnly(9, 0), CloseTime = new TimeOnly(18, 0), DisplayOrder = 2 },
        new() { Id = 3, BusinessProfileId = 1, DayOfWeek = DayOfWeek.Wednesday, IsClosed = false, OpenTime = new TimeOnly(9, 0), CloseTime = new TimeOnly(18, 0), DisplayOrder = 3 },
        new() { Id = 4, BusinessProfileId = 1, DayOfWeek = DayOfWeek.Thursday, IsClosed = false, OpenTime = new TimeOnly(9, 0), CloseTime = new TimeOnly(18, 0), DisplayOrder = 4 },
        new() { Id = 5, BusinessProfileId = 1, DayOfWeek = DayOfWeek.Friday, IsClosed = false, OpenTime = new TimeOnly(9, 0), CloseTime = new TimeOnly(18, 0), DisplayOrder = 5 },
        new() { Id = 6, BusinessProfileId = 1, DayOfWeek = DayOfWeek.Saturday, IsClosed = false, OpenTime = new TimeOnly(9, 0), CloseTime = new TimeOnly(16, 0), DisplayOrder = 6 },
        new() { Id = 7, BusinessProfileId = 1, DayOfWeek = DayOfWeek.Sunday, IsClosed = true, Notes = "By appointment", DisplayOrder = 7 }
    ];
}
