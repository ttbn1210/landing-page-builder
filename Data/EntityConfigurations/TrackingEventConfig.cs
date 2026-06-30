using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class TrackingEventConfig : IEntityTypeConfiguration<TrackingEvent>
{
    public void Configure(EntityTypeBuilder<TrackingEvent> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.EventType).HasConversion<string>().HasMaxLength(50);
        builder.Property(t => t.ElementName).HasMaxLength(200);
        builder.Property(t => t.Value).HasMaxLength(2000);

        builder.HasIndex(t => new { t.SessionId, t.OccurredAt });
    }
}
