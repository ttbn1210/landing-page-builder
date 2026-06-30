using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class LeadActivityConfig : IEntityTypeConfiguration<LeadActivity>
{
    public void Configure(EntityTypeBuilder<LeadActivity> builder)
    {
        builder.HasKey(la => la.Id);
        builder.Property(la => la.Action).IsRequired().HasMaxLength(200);
        builder.Property(la => la.Notes).HasMaxLength(2000);
    }
}
