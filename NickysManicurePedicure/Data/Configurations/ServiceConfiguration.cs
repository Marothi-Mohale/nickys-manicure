using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data.Configurations;

public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1200).IsRequired();
        builder.Property(x => x.DurationLabel).HasMaxLength(60).IsRequired();
        builder.Property(x => x.PriceFromLabel).HasMaxLength(60).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => new { x.ServiceCategoryId, x.DisplayOrder });
        builder.HasIndex(x => new { x.Status, x.IsFeatured, x.DisplayOrder });

        builder.HasOne(x => x.ServiceCategory)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
