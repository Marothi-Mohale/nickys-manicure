using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data.Configurations;

public sealed class BusinessProfileConfiguration : IEntityTypeConfiguration<BusinessProfile>
{
    public void Configure(EntityTypeBuilder<BusinessProfile> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Tagline).HasMaxLength(260).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(30).IsRequired();
        builder.Property(x => x.PhoneHref).HasMaxLength(60).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.AddressLine1).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Suburb).HasMaxLength(120).IsRequired();
        builder.Property(x => x.City).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Region).HasMaxLength(120).IsRequired();
        builder.Property(x => x.PostalCode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.WhatsAppHref).HasMaxLength(120).IsRequired();
        builder.Property(x => x.InstagramHandle).HasMaxLength(120);
        builder.Property(x => x.BookingPolicy).HasMaxLength(1000);
        builder.Property(x => x.AboutSummary).HasMaxLength(1200);
        builder.HasIndex(x => x.Email);

        builder.HasMany(x => x.BusinessHours)
            .WithOne(x => x.BusinessProfile)
            .HasForeignKey(x => x.BusinessProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
