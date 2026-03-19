using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data.Configurations;

public sealed class SalonServiceConfiguration : IEntityTypeConfiguration<SalonService>
{
    public void Configure(EntityTypeBuilder<SalonService> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(600).IsRequired();
        builder.Property(x => x.Duration).HasMaxLength(60).IsRequired();
        builder.Property(x => x.PriceFrom).HasMaxLength(60).IsRequired();
    }
}
