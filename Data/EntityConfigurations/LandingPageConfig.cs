using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class LandingPageConfig : IEntityTypeConfiguration<LandingPage>
{
    public void Configure(EntityTypeBuilder<LandingPage> builder)
    {
        builder.HasKey(lp => lp.Id);
        builder.Property(lp => lp.Name).IsRequired().HasMaxLength(200);
        builder.Property(lp => lp.TemplateType).HasConversion<string>().HasMaxLength(50);
        builder.Property(lp => lp.Status).HasConversion<string>().HasMaxLength(50);

        builder.HasMany(lp => lp.QRCodes)
            .WithOne(q => q.LandingPage)
            .HasForeignKey(q => q.LandingPageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
