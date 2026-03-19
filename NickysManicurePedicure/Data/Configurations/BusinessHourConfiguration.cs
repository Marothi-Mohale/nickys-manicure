using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Data.Configurations;

public sealed class BusinessHourConfiguration : IEntityTypeConfiguration<BusinessHour>
{
    public void Configure(EntityTypeBuilder<BusinessHour> builder)
    {
        builder.Property(x => x.DayOfWeek).HasConversion<int>();
        builder.Property(x => x.OpenTime);
        builder.Property(x => x.CloseTime);
        builder.Property(x => x.Notes).HasMaxLength(120);
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_BusinessHours_ClosedTimes",
                ConfigurationSql.BusinessHoursClosedTimes);
            table.HasCheckConstraint(
                "CK_BusinessHours_TimeRange",
                ConfigurationSql.BusinessHoursTimeRange);
        });
        builder.HasIndex(x => new { x.BusinessProfileId, x.DayOfWeek }).IsUnique();
    }
}
