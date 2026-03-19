using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data.Configurations;

public sealed class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.Property(x => x.ClientName).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Quote).HasMaxLength(600).IsRequired();
        builder.Property(x => x.Rating).IsRequired();
        builder.Property(x => x.IsApproved).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        builder.ToTable(t => t.HasCheckConstraint("CK_Testimonials_Rating", "\"Rating\" >= 1 AND \"Rating\" <= 5"));
        builder.HasIndex(x => new { x.Status, x.IsApproved, x.IsFeatured, x.DisplayOrder });
    }
}
