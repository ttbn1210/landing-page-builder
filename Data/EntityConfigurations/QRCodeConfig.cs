using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class QRCodeConfig : IEntityTypeConfiguration<QRCode>
{
    public void Configure(EntityTypeBuilder<QRCode> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.ShortCode).IsRequired().HasMaxLength(100);
        builder.HasIndex(q => q.ShortCode).IsUnique();
        builder.Property(q => q.Status).HasConversion<string>().HasMaxLength(50);

        builder.HasOne(q => q.Campaign)
            .WithMany(c => c.QRCodes)
            .HasForeignKey(q => q.CampaignId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.Placement)
            .WithMany(p => p.QRCodes)
            .HasForeignKey(q => q.PlacementId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.LandingPage)
            .WithMany(lp => lp.QRCodes)
            .HasForeignKey(q => q.LandingPageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(q => q.VisitorSessions)
            .WithOne(vs => vs.QRCode)
            .HasForeignKey(vs => vs.QRCodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.ScanEvents)
            .WithOne(se => se.QRCode)
            .HasForeignKey(se => se.QRCodeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
