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

    public static async Task EnsureDatabaseReadyAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitialization");
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var databaseOptions = scope.ServiceProvider
            .GetRequiredService<IOptions<DatabaseOptions>>()
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
}
