using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class VisitorSessionConfig : IEntityTypeConfiguration<VisitorSession>
{
    public void Configure(EntityTypeBuilder<VisitorSession> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.SessionKey).IsRequired().HasMaxLength(100);
        builder.HasIndex(v => v.SessionKey).IsUnique();
        builder.Property(v => v.DeviceFingerprint).HasMaxLength(500);
        builder.Property(v => v.IpAddress).HasMaxLength(50);
        builder.Property(v => v.UserAgent).HasMaxLength(1000);
        builder.Property(v => v.DeviceType).HasConversion<string>().HasMaxLength(50);
        builder.Property(v => v.VisitorSource).HasConversion<string>().HasMaxLength(50);

        builder.HasMany(v => v.TrackingEvents)
            .WithOne(te => te.Session)
            .HasForeignKey(te => te.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.Lead)
            .WithOne(l => l.Session)
            .HasForeignKey<Lead>(l => l.SessionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
