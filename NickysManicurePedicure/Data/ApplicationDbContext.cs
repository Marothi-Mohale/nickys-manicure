using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<FaqItem> FaqItems => Set<FaqItem>();
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

        modelBuilder.Entity<FaqItem>(entity =>
        {
            entity.Property(x => x.Question).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Answer).HasMaxLength(700).IsRequired();
        });

        modelBuilder.Entity<SalonService>().HasData(SeedDataExtensions.ServicesSeed);
        modelBuilder.Entity<Testimonial>().HasData(SeedDataExtensions.TestimonialsSeed);
        modelBuilder.Entity<FaqItem>().HasData(SeedDataExtensions.FaqSeed);
    }
}
