using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.Data;

public static class SeedDataExtensions
{
    public static IReadOnlyList<ServiceCategory> ServiceCategoriesSeed =>
    [
        new() { Id = 1, Name = "Manicure", Slug = "manicure", Description = "Luxury manicure rituals designed for elegant shape, healthy nails, and a polished finish.", Status = ContentStatus.Published, DisplayOrder = 1 },
        new() { Id = 2, Name = "Pedicure", Slug = "pedicure", Description = "Refined pedicure treatments that combine restorative care with premium presentation.", Status = ContentStatus.Published, DisplayOrder = 2 },
        new() { Id = 3, Name = "Enhancements", Slug = "enhancements", Description = "Premium add-ons and occasion-ready extras that elevate your final look.", Status = ContentStatus.Published, DisplayOrder = 3 }
    ];

    public static IReadOnlyList<Service> ServicesSeed =>
    [
        new() { Id = 1, ServiceCategoryId = 1, Name = "Luxury Signature Manicure", Slug = "luxury-signature-manicure", Description = "A full manicure ritual with precision cuticle care, tailored shaping, restorative hydration, and a flawless high-gloss finish suited to an elevated everyday look.", DurationLabel = "60 min", PriceFromLabel = "From R280", PriceFromAmount = 280m, IsFeatured = true, Status = ContentStatus.Published, DisplayOrder = 1 },
        new() { Id = 2, ServiceCategoryId = 2, Name = "Luxury Signature Pedicure", Slug = "luxury-signature-pedicure", Description = "An indulgent pedicure with soak, smoothing exfoliation, expert nail and cuticle care, and beautifully finished toes prepared for sandals and occasion wear.", DurationLabel = "75 min", PriceFromLabel = "From R340", PriceFromAmount = 340m, IsFeatured = true, Status = ContentStatus.Published, DisplayOrder = 2 },
        new() { Id = 3, ServiceCategoryId = 1, Name = "Gel Polish Refresh", Slug = "gel-polish-refresh", Description = "Long-wear gel color application for clients who want a polished, durable finish with clean lines and a salon-fresh look that lasts.", DurationLabel = "50 min", PriceFromLabel = "From R250", PriceFromAmount = 250m, IsFeatured = true, Status = ContentStatus.Published, DisplayOrder = 3 },
        new() { Id = 4, ServiceCategoryId = 2, Name = "Restorative Pedicure Therapy", Slug = "restorative-pedicure-therapy", Description = "A treatment-led pedicure focused on comfort, nourishment, callus refinement, and restoring tired feet to a polished, cared-for state.", DurationLabel = "70 min", PriceFromLabel = "From R320", PriceFromAmount = 320m, IsFeatured = false, Status = ContentStatus.Published, DisplayOrder = 4 },
        new() { Id = 5, ServiceCategoryId = 3, Name = "Nail Art Detail Work", Slug = "nail-art-detail-work", Description = "Refined custom accents and elevated design details tailored to your occasion, style direction, and desired finish.", DurationLabel = "30 - 45 min add-on", PriceFromLabel = "From R120", PriceFromAmount = 120m, IsFeatured = false, Status = ContentStatus.Published, DisplayOrder = 5 },
        new() { Id = 6, ServiceCategoryId = 1, Name = "Restorative Nail Care Session", Slug = "restorative-nail-care-session", Description = "A gentle manicure appointment designed for clients whose nails need nourishment, reshaping, and recovery after damage or product fatigue.", DurationLabel = "45 min", PriceFromLabel = "From R220", PriceFromAmount = 220m, IsFeatured = false, Status = ContentStatus.Published, DisplayOrder = 6 },
        new() { Id = 7, ServiceCategoryId = 3, Name = "Bridal Nail Styling Consultation", Slug = "bridal-nail-styling-consultation", Description = "A premium planning session for weddings and formal events, including finish direction, service planning, and design recommendations for a photo-ready result.", DurationLabel = "By consultation", PriceFromLabel = "Custom quote", PriceFromAmount = null, IsFeatured = false, Status = ContentStatus.Published, DisplayOrder = 7 }
    ];

    public static IReadOnlyList<Testimonial> TestimonialsSeed =>
    [
        new() { Id = 1, ClientName = "Lerato M.", Quote = "Nicky is warm, meticulous, and remarkably consistent. My manicure still looked elegant nearly two weeks later, and the whole appointment felt calm, polished, and genuinely luxurious.", Rating = 5, IsFeatured = true, IsApproved = true, Status = ContentStatus.Published, DisplayOrder = 1 },
        new() { Id = 2, ClientName = "Ayanda P.", Quote = "From the welcome to the final finish, the experience felt premium. The salon is spotless, the attention to detail is exceptional, and my pedicure looked refined enough for an event that same evening.", Rating = 5, IsFeatured = true, IsApproved = true, Status = ContentStatus.Published, DisplayOrder = 2 },
        new() { Id = 3, ClientName = "Zanele R.", Quote = "I booked before a special celebration and left feeling completely looked after. Nicky helped me choose a soft, elegant style that photographed beautifully and suited the occasion perfectly.", Rating = 5, IsFeatured = false, IsApproved = true, Status = ContentStatus.Published, DisplayOrder = 3 },
        new() { Id = 4, ClientName = "Nomsa T.", Quote = "I appreciate how thoughtful the service feels every time. My nails are healthy, beautifully shaped, and never rushed. It is the kind of beauty appointment that makes you feel quietly confident.", Rating = 5, IsFeatured = false, IsApproved = true, Status = ContentStatus.Published, DisplayOrder = 4 }
    ];

    public static IReadOnlyList<FaqItem> FaqSeed =>
    [
        new() { Id = 1, Question = "Do I need to book in advance?", Answer = "Advance booking is recommended, especially for Fridays, Saturdays, and pre-event appointments, so we can reserve the right amount of time for your service.", IsActive = true, Status = ContentStatus.Published, DisplayOrder = 1 },
        new() { Id = 2, Question = "Can I request a preferred date and time?", Answer = "Yes. Submit your preferred date and time in the booking form and we will confirm the closest available appointment as quickly as possible.", IsActive = true, Status = ContentStatus.Published, DisplayOrder = 2 },
        new() { Id = 3, Question = "Do you offer gel, classic polish, and custom finishes?", Answer = "Yes. Services can include classic polish, gel options, and refined design details depending on the look you want to achieve. If you have inspiration, you are welcome to mention it when booking.", IsActive = true, Status = ContentStatus.Published, DisplayOrder = 3 },
        new() { Id = 4, Question = "How should I prepare for my appointment?", Answer = "Arriving a few minutes early is ideal. If you currently have product on your nails or want a specific finish for an event, note that in your booking request so enough time can be scheduled.", IsActive = true, Status = ContentStatus.Published, DisplayOrder = 4 },
        new() { Id = 5, Question = "Where is the salon located?", Answer = "Nicky's Manicure & Pedicure is based at 72 Main Road, Mowbray, Cape Town, with a calm, private setting designed for a relaxed premium appointment.", IsActive = true, Status = ContentStatus.Published, DisplayOrder = 5 }
    ];

    public static IReadOnlyList<GalleryItem> GallerySeed =>
    [
        new() { Id = 1, Title = "Signature Nude Gloss", Description = "A refined nude manicure set with mirror-shine finishing and softly sculpted almond shape.", Category = "Manicure", ImageUrl = "/images/gallery/signature-nude-gloss.jpg", ThumbnailUrl = "/images/gallery/thumbs/signature-nude-gloss.jpg", AltText = "Luxury nude manicure with glossy finish and elegant almond shape", IsFeatured = true, Status = ContentStatus.Published, DisplayOrder = 1 },
        new() { Id = 2, Title = "Soft Bridal Detail", Description = "Bridal nail styling with clean structure, delicate accent work, and an understated premium finish.", Category = "Bridal", ImageUrl = "/images/gallery/soft-bridal-detail.jpg", ThumbnailUrl = "/images/gallery/thumbs/soft-bridal-detail.jpg", AltText = "Bridal manicure with soft detail accents and premium neutral finish", IsFeatured = true, Status = ContentStatus.Published, DisplayOrder = 2 },
        new() { Id = 3, Title = "Polished Pedicure Finish", Description = "A luxury pedicure presentation designed for sandal-ready elegance in neutral tones.", Category = "Pedicure", ImageUrl = "/images/gallery/polished-pedicure-finish.jpg", ThumbnailUrl = "/images/gallery/thumbs/polished-pedicure-finish.jpg", AltText = "Elegant pedicure in neutral tones with polished luxury finish", IsFeatured = false, Status = ContentStatus.Published, DisplayOrder = 3 },
        new() { Id = 4, Title = "French Line Minimal", Description = "A contemporary French manicure with crisp detailing, soft pink base, and editorial simplicity.", Category = "Manicure", ImageUrl = "/images/gallery/french-line-minimal.jpg", ThumbnailUrl = "/images/gallery/thumbs/french-line-minimal.jpg", AltText = "Minimal French manicure with crisp white tips and soft pink base", IsFeatured = false, Status = ContentStatus.Published, DisplayOrder = 4 },
        new() { Id = 5, Title = "Event Gloss Cocoa", Description = "A rich cocoa-toned set styled for evening events with a sleek high-gloss finish.", Category = "Occasion", ImageUrl = "/images/gallery/event-gloss-cocoa.jpg", ThumbnailUrl = "/images/gallery/thumbs/event-gloss-cocoa.jpg", AltText = "Glossy cocoa-toned manicure styled for an elegant evening event", IsFeatured = true, Status = ContentStatus.Published, DisplayOrder = 5 }
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

            await SeedMissingAsync(dbContext.ServiceCategories, ServiceCategoriesSeed, item => item.Id, "service categories", logger);
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
            profile.UpdatedAtUtc = DateTime.UtcNow;
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
