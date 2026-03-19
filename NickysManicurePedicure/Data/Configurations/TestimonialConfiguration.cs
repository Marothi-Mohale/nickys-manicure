using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data.Configurations;

public sealed class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.Property(x => x.ClientName).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Highlight).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Review).HasMaxLength(500).IsRequired();
    }
}
