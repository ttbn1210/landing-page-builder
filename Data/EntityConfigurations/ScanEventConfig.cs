using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class ScanEventConfig : IEntityTypeConfiguration<ScanEvent>
{
    public void Configure(EntityTypeBuilder<ScanEvent> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Country).HasMaxLength(100);
        builder.Property(s => s.Province).HasMaxLength(100);
        builder.Property(s => s.City).HasMaxLength(100);
        builder.Property(s => s.Referrer).HasMaxLength(500);

        builder.HasIndex(s => new { s.QRCodeId, s.OccurredAt });

        builder.HasOne(s => s.Session)
            .WithMany()
            .HasForeignKey(s => s.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
