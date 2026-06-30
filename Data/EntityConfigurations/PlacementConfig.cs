using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class PlacementConfig : IEntityTypeConfiguration<Placement>
{
    public void Configure(EntityTypeBuilder<Placement> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.LocationNote).HasMaxLength(500);
        builder.Property(p => p.Cost).HasColumnType("decimal(18,2)");

        builder.HasMany(p => p.QRCodes)
            .WithOne(q => q.Placement)
            .HasForeignKey(q => q.PlacementId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
