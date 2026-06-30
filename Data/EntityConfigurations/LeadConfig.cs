using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data.EntityConfigurations;

public class LeadConfig : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.FullName).HasMaxLength(200);
        builder.Property(l => l.Phone).HasMaxLength(50);
        builder.Property(l => l.Email).HasMaxLength(200);
        builder.Property(l => l.Message).HasMaxLength(2000);
        builder.Property(l => l.Status).HasConversion<string>().HasMaxLength(50);

        builder.HasIndex(l => l.Phone);

        builder.HasMany(l => l.Activities)
            .WithOne(a => a.Lead)
            .HasForeignKey(a => a.LeadId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
