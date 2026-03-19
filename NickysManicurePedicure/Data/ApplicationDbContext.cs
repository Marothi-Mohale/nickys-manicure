using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<BusinessHour> BusinessHours => Set<BusinessHour>();
    public DbSet<BusinessProfile> BusinessProfiles => Set<BusinessProfile>();
    public DbSet<FaqItem> FaqItems => Set<FaqItem>();
    public DbSet<GalleryItem> GalleryItems => Set<GalleryItem>();
    public DbSet<Inquiry> Inquiries => Set<Inquiry>();
    public DbSet<SalonService> Services => Set<SalonService>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Inquiry>(entity =>
        {
            entity.Property(x => x.FullName).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.PhoneNumber).HasMaxLength(30).IsRequired();
            entity.Property(x => x.PreferredService).HasMaxLength(120);
            entity.Property(x => x.Message).HasMaxLength(2000).IsRequired();
            entity.Property(x => x.SourcePage).HasMaxLength(40).IsRequired();
            entity.Property(x => x.InquiryType).HasConversion<string>().HasMaxLength(30);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            entity.HasIndex(x => x.CreatedUtc);
        });

        modelBuilder.Entity<BusinessProfile>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Tagline).HasMaxLength(260).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.PhoneHref).HasMaxLength(60).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.AddressLine1).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Suburb).HasMaxLength(120).IsRequired();
            entity.Property(x => x.City).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Region).HasMaxLength(120).IsRequired();
            entity.Property(x => x.PostalCode).HasMaxLength(20).IsRequired();
            entity.Property(x => x.WhatsAppHref).HasMaxLength(120).IsRequired();
            entity.Property(x => x.InstagramHandle).HasMaxLength(120);
            entity.Property(x => x.BookingPolicy).HasMaxLength(1000);
            entity.Property(x => x.AboutSummary).HasMaxLength(1200);
            entity.HasMany(x => x.BusinessHours)
                .WithOne(x => x.BusinessProfile)
                .HasForeignKey(x => x.BusinessProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BusinessHour>(entity =>
        {
            entity.Property(x => x.DayOfWeek).HasConversion<int>();
            entity.Property(x => x.Notes).HasMaxLength(120);
            entity.HasIndex(x => new { x.BusinessProfileId, x.DayOfWeek }).IsUnique();
        });

        modelBuilder.Entity<SalonService>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(600).IsRequired();
            entity.Property(x => x.Duration).HasMaxLength(60).IsRequired();
            entity.Property(x => x.PriceFrom).HasMaxLength(60).IsRequired();
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.Property(x => x.ClientName).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Highlight).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Review).HasMaxLength(500).IsRequired();
        });

        modelBuilder.Entity<GalleryItem>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(800);
            entity.Property(x => x.Category).HasMaxLength(60);
            entity.Property(x => x.ImageUrl).HasMaxLength(500);
            entity.Property(x => x.ThumbnailUrl).HasMaxLength(500);
            entity.Property(x => x.AltText).HasMaxLength(220);
            entity.HasIndex(x => new { x.IsPublished, x.DisplayOrder });
            entity.HasIndex(x => x.Category);
        });

        modelBuilder.Entity<FaqItem>(entity =>
        {
            entity.Property(x => x.Question).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Answer).HasMaxLength(700).IsRequired();
        });
    }
}
