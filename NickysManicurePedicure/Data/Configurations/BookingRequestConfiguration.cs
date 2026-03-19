using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data.Configurations;

public sealed class BookingRequestConfiguration : IEntityTypeConfiguration<BookingRequest>
{
    public void Configure(EntityTypeBuilder<BookingRequest> builder)
    {
        builder.Property(x => x.RequestedServiceName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.FullName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.SourcePage).HasMaxLength(40).IsRequired();
        builder.Property(x => x.AdminNotes).HasMaxLength(1000);
        builder.Property(x => x.PreferredDate).IsRequired();
        builder.Property(x => x.PreferredTime).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(x => x.CreatedAtUtc);
        builder.HasIndex(x => new { x.Status, x.PreferredDate, x.CreatedAtUtc });

        builder.HasOne(x => x.Service)
            .WithMany(x => x.BookingRequests)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
